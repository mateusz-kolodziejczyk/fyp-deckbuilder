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

    [SerializeField] private GameObject startEncounterPrefab, finalEncounterPrefab;
    
    private (Vector2Int pos, GameObject o) startEncounter, finalEncounter;
    
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
        var startEnc = GameObject.Instantiate(startEncounterPrefab, new Vector3(-encounterSpacing, pathSpacing, 0) , quaternion.identity);
        startEnc.transform.SetParent(map.transform, false);
        
        startEncounter = (new Vector2Int(-1, 0), startEnc);

        var p = 0;
        var i = 0;
        
        for (p = 0; p < nPaths; p++)
        {
            for (i = 0; i < length; i++)
            {
                var v = new Vector2Int(p, i);
                var encounter = GameObject.Instantiate(encounterPrefab, new Vector3(i*encounterSpacing, p*pathSpacing, 0) , quaternion.identity);
                encounter.transform.SetParent(map.transform, false);
                encounters[v] = encounter;
            }
        }
        
        var finalEnc = GameObject.Instantiate(encounterPrefab, new Vector3(i*encounterSpacing, pathSpacing, 0) , quaternion.identity);
        finalEnc.transform.SetParent(map.transform, false);
        finalEncounter = (new Vector2Int(i, 0), finalEnc);
    }

    private void InitialiseConnections()
    {
        connections[startEncounter.pos] = new HashSet<Vector2Int>();
        // Add connection from start encounter to every initial
        for (int i = 0; i < nPaths; i++)
        {
            connections[startEncounter.pos].Add(new Vector2Int(i, 0));
        }
        foreach (var encounterPos in encounters.Keys)
        {
            foreach (var direction in directions)
            {
                var pos = direction + encounterPos;
                if (!encounters.ContainsKey(pos) && pos != finalEncounter.pos)
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
        // Go through final nodes and add connections to final encounter
        
        for (int p = 0; p < nPaths; p++)
        {
            connections[new Vector2Int(p, length - 1)] = new HashSet<Vector2Int>{finalEncounter.pos};
        }
    }

    private void DisplayConnections()
    {
        DisplayStartConnections();
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

                if (!encounters.TryGetValue(connection, out _))
                {
                    continue;
                }

                var lineRenderer = lineRenderers[i];
                // Set the second point to where the other encounter is
                lineRenderer.SetPositions(new[]
                    {encounterObject.transform.position, encounters[connection].transform.position});
                i++;
            }
        }

        DisplayEndConnections();
    }

    private void DisplayStartConnections()
    {
        int i = 0;
        var lineRenderers = startEncounter.o.GetComponentsInChildren<LineRenderer>();
        if (!connections.TryGetValue(startEncounter.pos, out _))
        {
            return;
        }
        foreach (var connection in connections[startEncounter.pos])
        {
            if (i >= lineRenderers.Length)
            {
                break;
            }

            var lineRenderer = lineRenderers[i];
            // Set the second point to where the other encounter is
            lineRenderer.SetPositions(new []{startEncounter.o.transform.position, encounters[connection].transform.position});
            Debug.Log(connection);
            i++;
        }
    }

    private void DisplayEndConnections()
    {
        for (int p = 0; p < nPaths; p++)
        {
            var pos = new Vector2Int(p, length - 1);
            var go = encounters[pos];
            if (!connections.TryGetValue(pos, out _))
            {
                return;
            }
            
            var lineRenderers = go.GetComponentsInChildren<LineRenderer>();

            foreach (var connection in connections[pos])
            {
                var lineRenderer = lineRenderers[0];
                // Set the second point to where the other encounter is
                lineRenderer.SetPositions(new []{go.transform.position, finalEncounter.o.transform.position});
                Debug.Log(connection);
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

    private (bool, List<Vector2Int>) FindPath(Vector2Int start, Vector2Int destination)
    {
        return (false, null);
    }
}
