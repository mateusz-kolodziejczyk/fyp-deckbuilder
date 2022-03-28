using UnityEngine;
using UnityEngine.Tilemaps;

namespace Helper
{
    public static class GridHelper
    {
        public static (Vector3Int, bool) MousePosToGrid(Tilemap tilemap)
        {
            Vector2 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            Vector3Int gridPos = tilemap.WorldToCell(mousePos);
            if (!tilemap.HasTile(gridPos))
            {
                return (Vector3Int.zero, false);
            }

            return (gridPos, true);
        }
    }
}