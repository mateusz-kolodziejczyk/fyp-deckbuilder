using Character;
using UnityEngine;
using UnityEngine.Tilemaps;

namespace Movement
{
    [RequireComponent(typeof(CharacterData))]
    public class EnemyMovement : MonoBehaviour
    {
        [SerializeField]
        private Tilemap tilemap;
 
        private CharacterData characterData;

        private PlayerHolder playerHolder;
        // Start is called before the first frame update
        void Start()
        {
            playerHolder = GetComponent<PlayerHolder>();
            characterData = GetComponent<CharacterData>();
            // Reset enemy to start square
            Move();
        }

        // Update is called once per frame
        void Update()
        {
        
        }

        public void Move(Vector3Int direction = new ())
        {
            var nextCellPosition = characterData.Position + direction;
            if (!tilemap.HasTile(nextCellPosition))
            { 
                return;
            }
            var newPos = tilemap.CellToLocal(nextCellPosition);
            characterData.Position = nextCellPosition;
            transform.position = newPos;
        }
    }
}
