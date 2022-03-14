using System.Collections.Generic;
using UnityEngine;

namespace Helper
{
    public static class HelperConstants
    {
        public static  List<Vector3Int> adjacentAddition = new List<Vector3Int>() { 
            new(1,0, 0), 
            new(-1, 0, 0), 
            new(0, 1, 0), 
            new(0, -1, 0)
        };
    }
}