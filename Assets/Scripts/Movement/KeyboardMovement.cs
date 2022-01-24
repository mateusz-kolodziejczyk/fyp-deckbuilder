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
    public class KeyboardMovement : MonoBehaviour
    {
        // code taken from https://forum.unity.com/threads/need-help-with-grid-based-movement-on-isometric-z-as-y-tilemap.931605/
        [FormerlySerializedAs("_tilemap")] [SerializeField]
        private Tilemap tilemap;
 
        private CharacterData _characterData;
 
        // Start is called before the first frame update
        void Start()
        {
            _characterData = GetComponent<CharacterData>();
            // start at 0
            transform.position = tilemap.CellToLocal(_characterData.Position);
        }
 
        // Update is called once per frame
        void Update()
        {

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
            int distance = Math.Abs(_characterData.Position.x - gridPos.x) +
                           Math.Abs(_characterData.Position.y - gridPos.y);
            if (distance > _characterData.MovementSpeed)
            {
                return;
            }
            
            _characterData.Position = gridPos;
            Debug.Log(gridPos.x + "," + gridPos.y);
            var newPos = tilemap.CellToLocal(gridPos);
            transform.position = newPos;
            Debug.Log(tilemap.size);
        }
 
        void UpdateCurrentCell(Vector3Int offset)
        {
            _characterData.Position += offset;
            transform.position = tilemap.CellToLocal(_characterData.Position);
        }
    }
}
