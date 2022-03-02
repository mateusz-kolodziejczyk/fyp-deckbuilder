using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

namespace Helper
{
    public static class GridPainter
    {
        public static void PaintSquares(ref Tilemap tilemap,List<Vector3Int> squareCoords, Color color)
        {
            foreach (var coord in squareCoords)
            {
                tilemap.SetTileFlags(coord, TileFlags.None);
                tilemap.SetColor(coord, color);
            }

        }

        public static void ResetSquares(ref Tilemap tilemap, List<Vector3Int> squareCoords)
        {
            foreach (var coord in squareCoords)
            {
                tilemap.SetTileFlags(coord, TileFlags.None);
                tilemap.SetColor(coord, Color.white);
            }
        }

        public static void ResetTileMap(Tilemap tilemap)
        {
            
        }
    }
}
