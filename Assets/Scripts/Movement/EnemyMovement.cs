using System.Collections;
using System.Collections.Generic;
using Character;
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
        // Start is called before the first frame update
        void Start()
        {
            tilemap = GameObject.FindWithTag("TileMap").GetComponent<Tilemap>();
            
            playerHolder = GetComponent<PlayerHolder>();
            playerDataMono = playerHolder.GetPlayerData();
            
            characterDataMono = GetComponent<CharacterDataMono>();
            pathfinding = GetComponent<EnemyPathfinding>();
            // Reset enemy to start square
            ResetToStart();
        }

        // Update is called once per frame
        void Update()
        {
        
        }
        
        public void Move()
        {
            pathfinding.CalculatePath(characterDataMono.Position, playerDataMono.Position, tilemap, new List<Vector3Int>());

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
