using Character;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Tilemaps;
using MouseButton = UnityEngine.UIElements.MouseButton;

namespace Movement
{
    public class KeyboardMovement : MonoBehaviour
    {
        // code taken from https://forum.unity.com/threads/need-help-with-grid-based-movement-on-isometric-z-as-y-tilemap.931605/
        [SerializeField]
        private Tilemap _tilemap;
 
        private Vector3Int _currentCell;
 
        // Start is called before the first frame update
        void Start()
        {
            _currentCell = Vector3Int.zero;
            // start at 0
            transform.position = _tilemap.CellToLocal(_currentCell);
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
            Vector3Int gridPos = _tilemap.WorldToCell(mousePos);
            if (!_tilemap.HasTile(gridPos))
            {
                return;
            }
            _currentCell = gridPos;
            Debug.Log(gridPos.x + "," + gridPos.y);
            var newPos = _tilemap.CellToLocal(gridPos);
            transform.position = newPos;
            Debug.Log(_tilemap.size);
        }
 
        void UpdateCurrentCell(Vector3Int offset)
        {
            _currentCell += offset;
            transform.position = _tilemap.CellToLocal(_currentCell);
        }
    }
}
