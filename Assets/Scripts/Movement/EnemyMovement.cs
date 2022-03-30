using System.Collections;
using System.Collections.Generic;
using Character;
using UnityEngine;
using UnityEngine.Tilemaps;

namespace Movement
{
    [RequireComponent(typeof(CharacterData))]
    public class EnemyMovement : MonoBehaviour
    {
        private Tilemap tilemap;
 
        private CharacterData characterData;

        private PlayerHolder playerHolder;

        private EnemyPathfinding pathfinding;

        private CharacterData playerData;
        // Start is called before the first frame update
        void Start()
        {
            tilemap = GameObject.FindWithTag("TileMap").GetComponent<Tilemap>();
            
            playerHolder = GetComponent<PlayerHolder>();
            playerData = playerHolder.GetPlayerData();
            
            characterData = GetComponent<CharacterData>();
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
            pathfinding.CalculatePath(characterData.Position, playerData.Position, tilemap, new List<Vector3Int>());

            for (int i = 0; i < characterData.MovementSpeed; i++)
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
                if (pos == playerData.Position)
                {
                    return;
                }
                
                // Handle moving
                characterData.Position = pos;
                var newPos = tilemap.CellToLocal(pos);
                transform.position = newPos;
                
                Debug.Log("New Pos: " + pos);
            }
        }

        private void ResetToStart()
        {
            var pos = characterData.Position;
            // If tilemap doesn't have the tile, reset to 0
            if (!tilemap.HasTile(pos))
            {
                pos = Vector3Int.zero;
            }
            characterData.Position = pos;
            var newPos = tilemap.CellToLocal(pos);
            transform.position = newPos;
        }
    }
}
