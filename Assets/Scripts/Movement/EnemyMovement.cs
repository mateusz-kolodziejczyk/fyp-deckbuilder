using System.Collections.Generic;
using Character;
using Enemy;
using Managers;
using Player;
using UnityEngine;
using UnityEngine.Tilemaps;

namespace Movement
{
    [RequireComponent(typeof(CharacterDataMono))]
    public class EnemyMovement : MonoBehaviour
    {
        private Tilemap tilemap;
 
        private CharacterDataMono characterDataMono;

        private PlayerHolder playerHolder;

        private EnemyPathfinding pathfinding;

        private CharacterDataMono playerDataMono;

        private GameManager gameManager;
        // Start is called before the first frame update
        void Start()
        {
            tilemap = GameObject.FindWithTag("TileMap").GetComponent<Tilemap>();
            
            playerHolder = GetComponent<PlayerHolder>();
            playerDataMono = playerHolder.GetPlayerData();
            
            characterDataMono = GetComponent<CharacterDataMono>();
            pathfinding = GetComponent<EnemyPathfinding>();

            gameManager = GameObject.FindWithTag("GameController").GetComponent<GameManager>();
            
            // Reset enemy to start square
            ResetToStart();
        }

        // Update is called once per frame
        void Update()
        {
            if (playerDataMono == null)
            {
                playerDataMono = playerHolder.GetPlayerData();
            }
        }
        
        public void Move(List<Vector3Int> ignoredPositions)
        {
            pathfinding.CalculatePath(characterDataMono.Position, playerDataMono.Position, tilemap, ignoredPositions);

            // If gamemanager is not there, do not move.
            if (gameManager == null) return;
            for (int i = 0; i < characterDataMono.MovementSpeed; i++)
            {
                var (pos, returnedValue) = pathfinding.GetNextSquareInPath();
                // If didn't return a value, stop moving immediately
                if (!returnedValue)
                {
                    return;
                }

                if (!tilemap.HasTile(pos))
                {
                    return;
                    
                }
                
                // If the tile is the player's tile, do not move
                if (pos == playerDataMono.Position)
                {
                    return;
                }
                
                // If there is an enemy at the position, do not move
                if (gameManager.GetEnemyAtPosition(pos) != null)
                {
                    return;
                }
                // Handle moving
                characterDataMono.Position = pos;
                var newPos = tilemap.CellToLocal(pos);
                transform.position = newPos;
                
                Debug.Log("New Pos: " + pos);
            }
        }

        private void ResetToStart()
        {
            var pos = characterDataMono.Position;
            // If tilemap doesn't have the tile, reset to 0
            if (!tilemap.HasTile(pos))
            {
                pos = Vector3Int.zero;
            }
            characterDataMono.Position = pos;
            var newPos = tilemap.CellToLocal(pos);
            transform.position = newPos;
        }
    }
}
