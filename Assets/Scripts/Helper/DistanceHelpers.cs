using System;
using UnityEngine;

namespace Helper
{
    public class DistanceHelpers
    {
        public static int Vector3IntManhattanDistance(Vector3Int v1, Vector3Int v2)
        {
            return Math.Abs(v1.x - v2.x) +
                   Math.Abs(v1.y - v2.y);
        }
    }
}