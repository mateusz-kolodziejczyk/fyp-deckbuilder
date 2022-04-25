using System.Collections.Generic;
using System.Linq;
using Enums;
using ScriptableObjects;
using Statics;
using Unity.Mathematics;
using UnityEngine;
using Random = UnityEngine.Random;

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

    [SerializeField] [Range(0, 100)] private int allowedConsecutiveSkips;
    
    // This is a directional graph, so it is fine to do this
    private Dictionary<Vector2Int, HashSet<Vector2Int>> connections = new();

    [SerializeField] private GameObject encounterPrefab;

    [SerializeField] [Range(1, 10)] private float pathSpacing, encounterSpacing;

    [SerializeField] private GameObject map;

    [SerializeField] private GameObject startEncounterPrefab, finalEncounterPrefab;
    
    private (Vector2Int pos, GameObject o) startEncounter, finalEncounter;
    
    private int targetEdgesRemoved = 0;

    [SerializeField] [Range(1, 10)] private int maxShopsInARow, maxBattlesInARow;
    [SerializeField] [Range(0, 1)] private float percentShopsToChangeToBattles;
    
    [SerializeField] private List<ShopScriptableObject> shopScriptableObjects;
    [SerializeField] private List<BattleScriptableObject> battleScriptableObjects;
    
    [SerializeField] private List<EncounterScriptableObject> bossEncounterScriptableObjects;
    [SerializeField] private int seed;
    [SerializeField] private bool useSeed;
    
    // Properties
    public Dictionary<Vector2Int, GameObject> Encounters
    {
        get => encounters;
        set => encounters = value;
    }

    public (Vector2Int pos, GameObject o) StartEncounter
    {
        get => startEncounter;
        set => startEncounter = value;
    }

    public (Vector2Int pos, GameObject o) FinalEncounter
    {
        get => finalEncounter;
        set => finalEncounter = value;
    }

    // Start is called before the first frame update
    void Start()
    {
        if (useSeed)
        {
            Random.InitState(seed);
        }
        data = GetComponent<MapData>();

    }

    public void GenerateCampaignMap()
    {
        InitialiseMap();
        InitialiseConnections();

        // Sum the connections using LINQ
        var totalConnections = connections.Values.Select(x => x.Count).Sum();
        targetEdgesRemoved = (int)(totalConnections * percentEdgesRemoved);
        
        GenerateMap();
        PopulateEncounters();
        DisplayConnections();
    }

    public void LoadMap(Dictionary<Vector2Int, HashSet<Vector2Int>> connections, Dictionary<Vector2Int, EncounterScriptableObject> encounterScriptableObjects, EncounterScriptableObject bossEncounterScriptableObject)
    {
        // Generate base map
        InitialiseMap();
        // Load in the connections, and set the encounters up.
        this.connections = connections;
        
        foreach (var (pos, scriptableObject) in encounterScriptableObjects)
        {
            encounters[pos].GetComponent<EncounterData>().EncounterScriptableObject = scriptableObject;
        }

        finalEncounter.o.GetComponent<EncounterData>().EncounterScriptableObject = bossEncounterScriptableObject;
        
        // Display connection
        DisplayConnections();
        // Delete dangling vertices
        RemoveDanglingVertices();
    }

    public bool IsConnected(Vector2Int start, Vector2Int destination)
    {
        // Make sure the connections contain the start value, and that the destination also exists
        return connections.TryGetValue(start, out var adjacentConnections) && adjacentConnections.Contains(destination);
    }
    private void InitialiseMap()
    {
        var startEnc = GameObject.Instantiate(startEncounterPrefab, new Vector3(-encounterSpacing, pathSpacing, 0) , quaternion.identity);
        startEnc.transform.SetParent(map.transform, false);
        var startPos = new Vector2Int(0, -1);
        AddPositionToEncounterObject(startPos,ref startEnc);
        startEncounter = (startPos, startEnc);

        var p = 0;
        var i = 0;
        
        for (p = 0; p < nPaths; p++)
        {
            for (i = 0; i < length; i++)
            {
                var v = new Vector2Int(p, i);
                var encounter = GameObject.Instantiate(encounterPrefab, new Vector3(i*encounterSpacing, p*pathSpacing, 0) , quaternion.identity);
                encounter.transform.SetParent(map.transform, false);
                AddPositionToEncounterObject(v,ref encounter);
                encounters[v] = encounter;
            }
        }
        
        var finalEnc = GameObject.Instantiate(finalEncounterPrefab, new Vector3(i*encounterSpacing, pathSpacing, 0) , quaternion.identity);
        finalEnc.transform.SetParent(map.transform, false);
        var endPos = new Vector2Int(0, length);
        AddPositionToEncounterObject(endPos, ref finalEnc);
        finalEncounter = (endPos, finalEnc);
        encounters[endPos] = finalEnc;
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
            if (!encounters.ContainsKey(connection))
            {
                continue;
            }
            lineRenderer.SetPositions(new []{startEncounter.o.transform.position, encounters[connection].transform.position});
            i++;
        }
    }

    private void DisplayEndConnections()
    {
        for (int p = 0; p < nPaths; p++)
        {
            var pos = new Vector2Int(p, length - 1);
            if (!encounters.ContainsKey(pos))
            {
                continue;
            }
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
            }
        }

    }

    private void HideConnections()
    {
        
    }

    public void UpdateDataStore()
    {
        CampaignMapDataStore.Connections = connections;
        
        var posToEncounterScriptableObject = new Dictionary<Vector2Int, EncounterScriptableObject>();
        foreach (var (pos, o) in encounters)
        {
            posToEncounterScriptableObject[pos] = o.GetComponent<EncounterData>().EncounterScriptableObject;
        }

        CampaignMapDataStore.EncounterScriptableObjects = posToEncounterScriptableObject;
        CampaignMapDataStore.BossEncounterScriptableObject =
            finalEncounter.o.GetComponent<EncounterData>().EncounterScriptableObject;
        CampaignMapDataStore.FinalEncounterPos = finalEncounter.pos;
    }
    private void GenerateMap()
    {
        RemoveEdges();
        // After edges are removed, remove any vertices from the back that do not have any connections forwards
        RemoveDanglingVertices();
    }
    // Remove edges until the desired total is reached
    private void RemoveEdges()
    {
        var consecutiveSkips = 0;
        var edgesRemoved = 0;
        while (edgesRemoved < targetEdgesRemoved && consecutiveSkips < allowedConsecutiveSkips)
        {
            // Choose random start vertex
            var randomPos = new Vector2Int(Random.Range(0, nPaths), Random.Range(0, length));
            // Choose random connection from that vertex
            if (!connections.ContainsKey(randomPos))
            {
                continue;
            }
            
            var connectionsList = new List<Vector2Int>(connections[randomPos]);
            // If the pos has no connections left, return
            if (connectionsList.Count <= 0)
            {
                continue;
            }

            var connection = connectionsList[Random.Range(0, connectionsList.Count)];
            
            // Try to delete it, check if path to exit still exists from start, if not, restore it and continue
            connections[randomPos].Remove(connection);
            var pathFound = FindPath(startEncounter.pos, finalEncounter.pos);
            if (!pathFound)
            {
                connections[randomPos].Add(connection);
                consecutiveSkips++;
                continue;
            }
            // If there are no connections left, delete the node from both encounters and connections
            if (connections[randomPos].Count <= 0)
            {
                RemoveDanglingVertices();
            }
            // Reset consecutive skip count;
            consecutiveSkips = 0;
            edgesRemoved++;
        }
    }
    private void RemoveDanglingVertices()
    {
        // Go backwards
        for (int p = nPaths - 1; p >= 0; p--)
        {
            for (int i = length - 1; i >= 0; i--)
            {
                var pos = new Vector2Int(p, i);
                
                if (IsAlive(pos)) continue;
                
                if (encounters.ContainsKey(pos))
                {
                    encounters[pos].SetActive(false);
                    encounters.Remove(pos);
                }

                if (connections.ContainsKey(pos))
                {
                    connections.Remove(pos);
                }
                
                
            }
        }

        foreach (var key in connections.Keys)
        {
            connections[key].RemoveWhere(x => !connections.ContainsKey(x) && x != finalEncounter.pos);
        }
    }

    private void AddPositionToEncounterObject(Vector2Int position, ref GameObject encounterObject)
    {
        if (encounterObject.TryGetComponent(out EncounterData encounterData))
        {
            encounterData.Position = position;
        }
    }

    // Set all squares as inactive that are the same depth or less than the current player position
    public void SetActiveSquares(Vector2Int playerPos)
    {
        startEncounter.o.GetComponent<EncounterInteraction>().Active = false;

        foreach (var (pos, o) in encounters)       
        {
            if (o.TryGetComponent(out EncounterInteraction encounterInteraction))
            {
                if (pos.y <= playerPos.y)
                {
                    encounterInteraction.Active = false;
                }
                else
                {
                    encounterInteraction.Active = true;
                }
            }

        }
    }
    
    private bool IsAlive(Vector2Int node)
    {
        // If start node or end node, return alive
        if (node == startEncounter.pos || node == finalEncounter.pos)
        {
            return true;
        }
        // Check if node is alive
        // Conditions: Must have at least one node in, one node out
        // Node out
        if (connections.TryGetValue(node, out var localConnections))
        {
            if (localConnections.Count <= 0)
            {
                return false;
            }
        }
        else
        {
            return false;
        }
        // Node in
        // Go through every value in connections and make sure the node is in at least one connection
        return connections.Values.Any(x => x.Contains(node));
    }
    private bool FindPath(Vector2Int start, Vector2Int destination)
    {
        // Use DFS to find a path
        var toVisit = new Stack<Vector2Int>();
        toVisit.Push(start);
        var discovered = new HashSet<Vector2Int>();
        while (toVisit.Count > 0)
        {

            var node = toVisit.Pop();
            // Check if vlaue is destination
            if (node == destination)
            {
                return true;
            }
            
            if (discovered.Contains(node))
            {
                continue;
            }

            discovered.Add(node);
            if (connections.TryGetValue(node, out var localConnections))
            {
                foreach (var connection in localConnections)
                {
                    toVisit.Push(connection);
                }
            }
        }
        return false;
    }

    private void PopulateEncounters()
    {
        var rowTypes = GenerateRowTypes();
        var (battleLookupTable, battleMax) = GetEncounterLookupTable(new(battleScriptableObjects));
        var (shopLookupTable, shopMax) = GetEncounterLookupTable(new(shopScriptableObjects));

        foreach (var (pos, encounter) in encounters)
        {
            // Pick lookup table based on row of current encounter
            Dictionary<int, EncounterScriptableObject> lookupTable = new();
            var max = 0;
            var rowsWithShopsRemoved = new HashSet<int>();
            if (rowTypes.TryGetValue(pos.y, out var type))
            {
                switch (type)
                {
                    case EncounterType.Battle:
                        lookupTable = battleLookupTable;
                        max = battleMax;
                        break;
                    case EncounterType.Shop:
                        lookupTable = shopLookupTable;
                        max = shopMax;
                        // Roll a random number, if below % replace
                        var rnd = Random.value;
                        // Also check that not too many shops were removed, only remove one per row.
                        if (rnd < percentShopsToChangeToBattles && !rowsWithShopsRemoved.Contains(pos.y))
                        {
                            lookupTable = battleLookupTable;
                            max = battleMax;
                            rowsWithShopsRemoved.Add(pos.y);
                        }
                        break;
                    default:
                        break;
                }
            }
            
            var randomValue = Random.Range(0, max);
            EncounterScriptableObject chosenEncounter = null;
            foreach (var key in lookupTable.Keys.ToList().OrderBy(x => x))
            {
                if (randomValue >= key) continue;
                
                chosenEncounter = lookupTable[key];
                break;
            }

            if (chosenEncounter == null)
            {
                continue;
            }

            encounter.GetComponent<EncounterData>().EncounterScriptableObject = chosenEncounter;
        }

        finalEncounter.o.GetComponent<EncounterData>().EncounterScriptableObject =
            bossEncounterScriptableObjects[Random.Range(0, bossEncounterScriptableObjects.Count)];
    }

    private (Dictionary<int, EncounterScriptableObject>, int max) GetEncounterLookupTable(List<EncounterScriptableObject> encounterScriptableObjects)
    {
        var currentMax = 0;

        var lookupTable = new Dictionary<int, EncounterScriptableObject>();
        foreach (var o in encounterScriptableObjects)
        {
            currentMax += (int) (((float)1 / (float)o.weight) * (float)1000);
            lookupTable[currentMax] = o;
        }

        return (lookupTable, currentMax);
    }

    private Dictionary<int, EncounterType> GenerateRowTypes()
    {
        var shopsInARow = 0;
        var battlesInARow = 0;

        var rowTypes = new Dictionary<int, EncounterType>();
        for (var i = 0; i < length; i++)
        {
            // Each row has a 40/60 chance of being shop/battle
            var randomValue = Random.value;
            var isBattle = randomValue < 0.6;
            if (!isBattle)
            {
                // Only add new shop if there aren't already too many shops in a row
                if (shopsInARow >= maxShopsInARow)
                {
                    shopsInARow = 0;
                    rowTypes.Add(i, EncounterType.Battle);
                    battlesInARow++;
                }
                else
                {
                    battlesInARow = 0;
                    rowTypes.Add(i, EncounterType.Shop);
                    shopsInARow++;
                }

            }
            else
            {
                if (battlesInARow >= maxBattlesInARow)
                {
                    battlesInARow = 0;
                    rowTypes.Add(i, EncounterType.Shop);
                    shopsInARow++;
                }
                else
                {
                    shopsInARow = 0;
                    rowTypes.Add(i, EncounterType.Battle);
                    battlesInARow++;
                }
            }
        }
        return rowTypes;
    }
    
}
