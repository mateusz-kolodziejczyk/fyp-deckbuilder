using System;
using System.Collections.Generic;
using Character;
using Helper;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.Tilemaps;
using MouseButton = UnityEngine.UIElements.MouseButton;

namespace Movement
{
    [RequireComponent(typeof(CharacterData))]
    [RequireComponent(typeof(PlayerTurn))]
    public class PlayerMovement : MonoBehaviour
    {
        // code taken from https://forum.unity.com/threads/need-help-with-grid-based-movement-on-isometric-z-as-y-tilemap.931605/
        [SerializeField]
        private Tilemap tilemap;
 
        private CharacterData characterData;

        private PlayerTurn playerTurn;

        private bool isShowingMovementRange = false;

        private HashSet<Vector3Int> movableSquares = new HashSet<Vector3Int>();
        // Start is called before the first frame update
        void Start()
        {
            playerTurn = GetComponent<PlayerTurn>();
            
            characterData = GetComponent<CharacterData>();
            // start at 0
            transform.position = tilemap.CellToLocal(characterData.Position);
        }
 
        // Update is called once per frame
        void Update()
        {


            if (!playerTurn.IsPlayerTurn())
            {
                return;
            }
            
            if (isShowingMovementRange == false)
            {
                ShowMovementRange();
                isShowingMovementRange = true;
            }
            
            if (Input.GetMouseButtonDown(0))
            {
                UpdateCurrentCellMouse();
            }

        }

        void UpdateCurrentCellMouse()
        {
            if (Camera.main is null)
            {
                return;
            }
            Vector2 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            Vector3Int gridPos = tilemap.WorldToCell(mousePos);
            if (!tilemap.HasTile(gridPos))
            { 
                return;
            }
            // Get the manhattan distance to calculate where the player can move
            int distance = DistanceHelpers.Vector3IntManhattanDistance(characterData.Position, gridPos);
            if (distance > characterData.MovementPoints)
            {
                return;
            }
            
            characterData.Position = gridPos;
            Debug.Log(gridPos.x + "," + gridPos.y);
            var newPos = tilemap.CellToLocal(gridPos);
            transform.position = newPos;
            
            // Cleanup the movement range visual
            CleanupMovementRange();
            isShowingMovementRange = false;
            
            // Lower Available movement points
            characterData.UseMovementPoints(distance);
        }
 
        void UpdateCurrentCell(Vector3Int offset)
        {
            characterData.Position += offset;
            transform.position = tilemap.CellToLocal(characterData.Position);
        }

        private void ShowMovementRange()
        {
            var startPos = characterData.Position;
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
                    // If current node's distance to the start is the same as movement points, return
                    if (DistanceHelpers.Vector3IntManhattanDistance(pos, startPos) >= characterData.MovementPoints)
                    {
                        return;
                    }
                    visited.Add(pos);
                    foreach (var adjacentAdd in adjacentAddition)
                    {
                        var adjacent = adjacentAdd + pos;
                        if (tilemap.HasTile(adjacent))
                        {
                            tilemap.SetTileFlags(adjacent, TileFlags.None);
                            tilemap.SetColor(adjacent, Color.magenta);
                            movableSquares.Add(adjacent);
                            queue.Enqueue(adjacent);
                        }
                        
                    }
                }
            }
        }

        private void CleanupMovementRange()
        {
            foreach (var movableSquare in movableSquares)
            {
                tilemap.SetColor(movableSquare, Color.white);
            }
            movableSquares.Clear();
        }
    }
}
