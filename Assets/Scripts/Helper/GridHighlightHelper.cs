using System.Collections.Generic;
using System.Linq;
using ScriptableObjects.ScriptableObjects;
using Unity.VisualScripting;
using UnityEngine;

namespace Helper
{
    public static class GridHighlightHelper
    {
        public static List<Vector3Int> CalculateHighlightedSquares(Vector3Int startPosition, int range)
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

        public static List<Vector3Int> CalculateHighlightedSquares(Vector3Int startPosition, int range,
            TargetingPatternScriptableObject targetingPattern)
        {
            var highlightedSquares = new List<Vector3Int>();
            var directions = HelperConstants.adjacentAddition;
            // Turn the vector2int into a vector3int using LINQ
            foreach (var direction in targetingPattern.directions.Select(v => new Vector3Int(v.x, v.y, 0)))
            {
                // If the pattern doesn't repeat, only add one value and ignore range
                if (!targetingPattern.repeat)
                {
                    highlightedSquares.Add(direction + startPosition);
                    continue;
                }
                
                for (int i = 1; i <= range; i++)
                {
                    highlightedSquares.Add(i * direction + startPosition);
                }
            }

            return highlightedSquares;
        }
    }
}