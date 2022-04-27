using System.Collections.Generic;
using Helper;
using UnityEngine;
using UnityEngine.Tilemaps;

namespace Enemy
{
    public class EnemyPathfinding : MonoBehaviour
    {
        private Queue<Vector3Int> path = new();

        // Start is called before the first frame update
        void Start()
        {
        }

        // Update is called once per frame
        void Update()
        {
            
        }

        public void CalculatePath(Vector3Int origin, Vector3Int target, Tilemap tilemap,
            List<Vector3Int> ignoredPositions)
        {
            path = Algorithms.AStar(origin, target, tilemap, ignoredPositions);
        }
        public (Vector3Int, bool) GetNextSquareInPath()
        {

            // Only run if path contains
            if (path.Count > 0)
            {
                return (path.Dequeue(), true);
            }

            return (Vector3Int.zero, false);
        }
    }
}
