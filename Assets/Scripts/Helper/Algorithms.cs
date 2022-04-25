using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Tilemaps;
using static System.Int32;

namespace Helper
{
    public static class Algorithms
    {
        public static Queue<Vector3Int> AStar(Vector3Int origin, Vector3Int target, Tilemap tilemap, List<Vector3Int> ignoredPositions, bool stopBeforeTarget = true)
        {
            var openSet = new HashSet<Vector3Int>();
            openSet.Add(origin);
            var cameFrom = new Dictionary<Vector3Int, Vector3Int>();

            var g = new Dictionary<Vector3Int, int>();
            g[origin] = 0;
            var f = new Dictionary<Vector3Int, int>();
            f[origin] = DistanceHelpers.Vector3IntManhattanDistance(origin, target);
            
            var current = new Vector3Int(MaxValue, MaxValue, target.z);

            // As long as the openset isn't empty, continue
            while (openSet.Count > 0)
            {
                current = new Vector3Int(MaxValue, MaxValue, target.z);
                var (newCurrent, foundTarget) = RecalculateCurrent(openSet, f, target, current);
                
                current = newCurrent;
                if (foundTarget)
                {
                    break;
                }

                openSet.Remove(current);
                var adj = FindAdjacent(tilemap, current);
                foreach (var pos in adj)
                {
                    // Make sure the pos isn't the start
                    if (pos == origin)
                    {
                        continue;
                    }
                    
                    // Make sure position isn't in ignored positions
                    if (ignoredPositions.Contains(pos))
                    {
                        continue;
                    }

                    var tentativeG = g[current] + 1;
                    if (!g.ContainsKey(pos))
                    {
                        g[pos] = MaxValue;
                    }
                    
                    // Shortest path if it hasn't been visited yet, or if its distance to start is less than previous to it
                    if (tentativeG < g[pos])
                    {
                        cameFrom[pos] = current;
                        g[pos] = tentativeG;
                        f[pos] = tentativeG + DistanceHelpers.Vector3IntManhattanDistance(pos, target);
                        
                        // Attempt to insert neighbour into the set
                        openSet.Add(pos);
                    }
                }

            }
            return ReconstructPath(cameFrom, current);
        }

        private static Queue<Vector3Int> ReconstructPath(Dictionary<Vector3Int, Vector3Int> cameFrom, Vector3Int current)
        {
            var path = new Queue<Vector3Int>();
            path.Enqueue(current);

            while (cameFrom.ContainsKey(current))
            {
                current = cameFrom[current];
                path.Enqueue(current);
            }


            // Reverse as the path will be backwards to begin with
            path = new Queue<Vector3Int>(path.Reverse());
            
            if (path.Count > 0)
            {
                path.Dequeue();
            }
            return path;
        }

        private static (Vector3Int current, bool foundTarget) RecalculateCurrent(HashSet<Vector3Int> openSet, Dictionary<Vector3Int, int> f, Vector3Int target, Vector3Int current)
        {
            foreach (var pos in openSet)
            {
                if (!f.ContainsKey(current))
                {
                    f[current] = MaxValue;
                }
                if(f[pos] < f[current] || pos == target){
                    current = pos;
                }
                // If target is reach, break the loop
                if(current == target)
                {
                    return (current, true);
                }
            }

            return (current, false);
        }

        public static List<Vector3Int> FindAdjacent(Tilemap tilemap, Vector3Int pos)
        {
            var adjacent = new List<Vector3Int>();

            foreach (var direction in HelperConstants.adjacentAddition)
            {
                var newGridPos = pos + direction;
                if (tilemap.HasTile(newGridPos))
                {
                    adjacent.Add(newGridPos);
                }
            }

            return adjacent;
        }
    }
}