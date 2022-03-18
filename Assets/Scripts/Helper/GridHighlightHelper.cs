using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

namespace Helper
{
    public static class GridHighlightHelper
    {
        public static List<Vector3Int> CalculateHightlightedSquares(Vector3Int startPosition, int range)
        {
            var highlightedSquares = new List<Vector3Int>();
            var directions = HelperConstants.adjacentAddition;
            foreach (var direction in directions)
            {
                for (int i = 1; i <= range; i++)
                {
                    highlightedSquares.Add(i * direction + startPosition);
                }
            }

            return highlightedSquares;
        }
    }
}