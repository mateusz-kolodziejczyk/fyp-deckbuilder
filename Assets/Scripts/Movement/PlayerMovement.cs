using System;
using Character;
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
            int distance = Math.Abs(characterData.Position.x - gridPos.x) +
                           Math.Abs(characterData.Position.y - gridPos.y);
            if (distance > characterData.MovementSpeed)
            {
                return;
            }
            
            characterData.Position = gridPos;
            Debug.Log(gridPos.x + "," + gridPos.y);
            var newPos = tilemap.CellToLocal(gridPos);
            transform.position = newPos;
            Debug.Log(tilemap.size);
        }
 
        void UpdateCurrentCell(Vector3Int offset)
        {
            characterData.Position += offset;
            transform.position = tilemap.CellToLocal(characterData.Position);
        }
    }
}
