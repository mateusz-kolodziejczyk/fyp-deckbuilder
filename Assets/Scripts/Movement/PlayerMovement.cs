using System;
using System.Collections.Generic;
using System.Linq;
using Character;
using Enums;
using Helper;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.Tilemaps;
using MouseButton = UnityEngine.UIElements.MouseButton;

namespace Movement
{
    [RequireComponent(typeof(CharacterDataMono))]
    [RequireComponent(typeof(PlayerTurn))]
    public class PlayerMovement : MonoBehaviour
    {
        // code taken from https://forum.unity.com/threads/need-help-with-grid-based-movement-on-isometric-z-as-y-tilemap.931605/
        [SerializeField]
        private Tilemap tilemap;
 
        private CharacterDataMono characterDataMono;

        private PlayerTurn playerTurn;

        private bool isShowingMovementRange = false;

        private HashSet<Vector3Int> movableSquares = new HashSet<Vector3Int>();

        [SerializeField]
        private TextMeshProUGUI movementPointsText;

        private DrawSquares drawSquares;
        
        // Start is called before the first frame update
        void Start()
        {
            playerTurn = GetComponent<PlayerTurn>();
            
            characterDataMono = GetComponent<CharacterDataMono>();
            // start at 0
            transform.position = tilemap.CellToLocal(characterDataMono.Position);

            movementPointsText.text = $"{characterDataMono.MovementSpeed}/{characterDataMono.MovementSpeed}";
            
            var gridDrawerController = GameObject.FindWithTag("GridDrawerController");
            if (gridDrawerController != null)
            {
                if (gridDrawerController.TryGetComponent(out DrawSquares drawSquares))
                {
                    this.drawSquares = drawSquares;
                }
            }
        }
 
        // Update is called once per frame
        void Update()
        {
            if (!playerTurn.IsPlayerTurn())
            {
                return;
            }

            movementPointsText.text = $"{characterDataMono.MovementPoints}/{characterDataMono.MovementSpeed}";
        }

        public void UpdateCurrentCellMouse(Vector3Int pos)
        {
            if (Camera.main is null)
            {
                return;
            }

            // Get the manhattan distance to calculate where the player can move
            int distance = DistanceHelpers.Vector3IntManhattanDistance(characterDataMono.Position, pos);
            if (distance > characterDataMono.MovementPoints)
            {
                return;
            }
            
            characterDataMono.Position = pos;
            UpdatePositionToDataPosition();
            
            // Cleanup the movement range visual
            CleanupMovementRange();
            
            // Lower Available movement points
            characterDataMono.UseMovementPoints(distance);
        }

        public void UpdatePositionToDataPosition()
        {
            var pos = characterDataMono.Position;
            if (!tilemap.HasTile(pos))
            {
                characterDataMono.Position = Vector3Int.zero;
                return;
            }
            
            var newPos = tilemap.CellToLocal(pos);
            transform.position = newPos;
        }
 
        void UpdateCurrentCell(Vector3Int offset)
        {
            characterDataMono.Position += offset;
            transform.position = tilemap.CellToLocal(characterDataMono.Position);
        }

        public void ShowMovementRange(List<Vector3Int> enemyPositions)
        {
            // Do not recalculate movement range if it has already been done
            if (isShowingMovementRange)
            {
                return;
            }
            var startPos = characterDataMono.Position;
            // Color moveable tiles black

            // Start a breadth first search starting from the player pos and ending when movement speed is expended.
            var visited = new HashSet<Vector3Int>();
            var queue = new Queue<Vector3Int>();
            queue.Enqueue(startPos);
            
            // This list contains the vectors that are added to the current position to get its four directly adjacent squares.
            var adjacentAddition = new List<Vector3Int>() { 
                new(1,0, 0), 
                new(-1, 0, 0), 
                new(0, 1, 0), 
                new(0, -1, 0)
            };
            
            while (queue.Count > 0)
            {
                var pos = queue.Dequeue();
                if (!visited.Contains(pos))
                {
                    // If current node's distance to the start is the same as movement points, break
                    if (DistanceHelpers.Vector3IntManhattanDistance(pos, startPos) >= characterDataMono.MovementPoints)
                    {
                        break;
                    }
                    visited.Add(pos);
                    foreach (var adjacentAdd in adjacentAddition)
                    {
                        var adjacent = adjacentAdd + pos;
                        // Make sure the tile does nto have an enemy on it
                        if (!tilemap.HasTile(adjacent) || enemyPositions.Contains(adjacent)) continue;
                        
                        movableSquares.Add(adjacent);
                        queue.Enqueue(adjacent);

                    }
                }
            }

            if (drawSquares != null)
            {
                drawSquares.DrawHighlights(movableSquares.ToList(), HighlightType.PlayerMovement);
            }
            
            // Set the flag to true
            isShowingMovementRange = true;
        }

        public void CleanupMovementRange()
        {
            if (!isShowingMovementRange)
            {
                return;
            }
            if (drawSquares != null)
            {
                drawSquares.ResetHighlights(movableSquares.ToList(), HighlightType.PlayerMovement);
            }            
            movableSquares.Clear();
            // Reset flag
            isShowingMovementRange = false;
        }
    }
}
