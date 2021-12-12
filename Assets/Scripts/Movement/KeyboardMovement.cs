using Character;
using UnityEngine;
using UnityEngine.Tilemaps;

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
            if (Input.GetKeyDown(KeyCode.W))
            {
                UpdateCurrentCell(Vector3Int.up);
            }
            if (Input.GetKeyDown(KeyCode.A))
            {
                UpdateCurrentCell(Vector3Int.left);
            }
            if (Input.GetKeyDown(KeyCode.S))
            {
                UpdateCurrentCell(Vector3Int.down);
            }
            if (Input.GetKeyDown(KeyCode.D))
            {
                UpdateCurrentCell(Vector3Int.right);
            }
        }
 
        void UpdateCurrentCell(Vector3Int offset)
        {
            _currentCell += offset;
            transform.position = _tilemap.CellToLocal(_currentCell);
        }
    }
}
