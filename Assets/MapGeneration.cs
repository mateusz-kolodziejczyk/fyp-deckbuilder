using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using Unity.Mathematics;
using UnityEngine;

[RequireComponent(typeof(MapData))]
public class MapGeneration : MonoBehaviour
{
    // Holds the directions for the connections
    private List<Vector2Int> directions = new()
    {
        new Vector2Int(1,1),
        new Vector2Int(-1,1),
        new Vector2Int(0,1),
    };
    private MapData data;

    private Dictionary<Vector2Int, GameObject> encounters = new();

    [SerializeField] [Range(1, 5)] private int nPaths;
    [SerializeField] [Range(1, 50)] private int length;

    [SerializeField] [Range(0f, 1f)] private float percentEdgesRemoved;
    // This is a directional graph, so it is fine to do this
    private Dictionary<Vector2Int, HashSet<Vector2Int>> connections = new();

    [SerializeField] private GameObject encounterPrefab;

    [SerializeField] [Range(1, 10)] private float pathSpacing, encounterSpacing;

    [SerializeField] private GameObject map;

    private GameObject startEncounter;
    private GameObject finalEncounter;
    private int targetEdgesRemoved = 0;
    
    // Start is called before the first frame update
    void Start()
    {
        data = GetComponent<MapData>();
        InitialiseMap();
        InitialiseConnections();
        DisplayConnections();
        // Sum the connections using LINQ
        var totalConnections = connections.Values.Select(x => x.Count).Sum();
        targetEdgesRemoved = (int)(totalConnections * percentEdgesRemoved);
    }
    
    private void InitialiseMap()
    {
        for (int p = 0; p < nPaths; p++)
        {
            for (int i = 0; i < length; i++)
            {
                var v = new Vector2Int(p, i);
                var encounter = GameObject.Instantiate(encounterPrefab, new Vector3(i*encounterSpacing, p*pathSpacing, 0) , quaternion.identity);
                encounter.transform.SetParent(map.transform, false);
                encounters[v] = encounter;
            }
        }
    }

    private void InitialiseConnections()
    {
        foreach (var encounterPos in encounters.Keys)
        {
            foreach (var direction in directions)
            {
                var pos = direction + encounterPos;
                if (!encounters.ContainsKey(pos))
                {
                    continue;
                }

                // If hashset doesn't exist yet, create it
                if (!connections.TryGetValue(encounterPos, out _))
                {
                    connections[encounterPos] = new HashSet<Vector2Int>();
                }
                
                connections[encounterPos].Add(pos);
            }
        }
    }

    private void DisplayConnections()
    {
        foreach (var (pos, encounterObject) in encounters)
        {
            // Go through each encounter, and set each of its three children to a connected value
            // This is the index of the line currently considered
            int i = 0;
            var lineRenderers = encounterObject.GetComponentsInChildren<LineRenderer>();
            if (!connections.TryGetValue(pos, out _))
            {
                continue;
            }
            foreach (var connection in connections[pos])
            {
                if (i >= lineRenderers.Length)
                {
                    break;
                }

                var lineRenderer = lineRenderers[i];
                // Set the second point to where the other encounter is
                lineRenderer.SetPositions(new []{encounterObject.transform.position, encounters[connection].transform.position});
                Debug.Log(connection);
                i++;
            }
        }
    }

    private void HideConnections()
    {
        
    }

    // Remove edges until the desired total is reached
    private void RemoveEdges()
    {
        // Choose random start vertex
        // Choose random connection from that vertex
        // Try to delete it, check if path to exit still exists from start
    }
}
