using System.Collections.Generic;
using Enums;
using UnityEngine;
using UnityEngine.Tilemaps;

namespace Character
{
    [RequireComponent(typeof(EnemyAttack))]
    public class Intent : MonoBehaviour
    {
        private Tilemap tilemap;
        private EnemyDataMono dataMono;
        private List<Vector3Int> paintedTiles;
        private EnemyAttack enemyAttack;
        private DrawSquares drawSquares;

        private void Start()
        {
            tilemap = GameObject.FindWithTag("TileMap").GetComponent<Tilemap>();
            dataMono = GetComponent<EnemyDataMono>();
            paintedTiles = new List<Vector3Int>();
            enemyAttack = GetComponent<EnemyAttack>();
        
            var gridDrawerController = GameObject.FindWithTag("GridDrawerController");
            if (gridDrawerController != null)
            {
                if (gridDrawerController.TryGetComponent(out DrawSquares drawSquares))
                {
                    this.drawSquares = drawSquares;
                }
            }
        }

        public void DrawIntent()
        {
            var squaresToPaint = new List<Vector3Int>(enemyAttack.SquaresToAttack);
            if (drawSquares != null)
            {
                drawSquares.DrawHighlights(squaresToPaint, HighlightType.EnemyAttack);
            }
        }
    }
}
