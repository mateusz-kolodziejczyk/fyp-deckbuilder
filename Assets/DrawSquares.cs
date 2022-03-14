using System.Collections;
using System.Collections.Generic;
using Enums;
using Helper;
using UnityEngine;
using UnityEngine.Tilemaps;

public class DrawSquares : MonoBehaviour
{
    private Dictionary<Vector3Int, GameObject> playerHighlights = new();
    private Dictionary<Vector3Int, GameObject> enemyHighlights = new();

    private Dictionary<EntityType, Dictionary<Vector3Int, GameObject>> entityToHighlights = new();

    [SerializeField] private GameObject playerHighlight;
    [SerializeField] private GameObject enemyHighlight;

    [SerializeField] private Tilemap tilemap;

    private float yOffset = 0.25f;
    // Start is called before the first frame update
    void Start()
    {
        InitializeHighlights();
        entityToHighlights[EntityType.Player] = playerHighlights;
        entityToHighlights[EntityType.Enemy] = enemyHighlights;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void InitializeHighlights()
    {
        var toVisit = new Queue<Vector3Int>();
        // If the tilemap is empty, don't do anything
        if (!tilemap.HasTile(Vector3Int.zero))
        {
            return;
        }
        
        toVisit.Enqueue(Vector3Int.zero);
        
        var visited = new HashSet<Vector3Int>();

        // Go through each child until no more tiles in the tilemap
        while (toVisit.Count > 0)
        {
            var pos = toVisit.Dequeue();
            visited.Add(pos);

            var worldPos = new Vector3(tilemap.CellToWorld(pos).x, tilemap.CellToWorld(pos).y + yOffset,
                tilemap.CellToWorld(pos).z);
            // Draw highlight at position.
            var newPlayerHighlight = Instantiate(playerHighlight, worldPos, Quaternion.identity);
            var newEnemyHighlight = Instantiate(enemyHighlight, worldPos, Quaternion.identity);
            
            playerHighlights[pos] = newPlayerHighlight;
            enemyHighlights[pos] = newEnemyHighlight;
            
            newPlayerHighlight.SetActive(false);
            newEnemyHighlight.SetActive(false);
            
            foreach (var adjacentAdd in HelperConstants.adjacentAddition)
            {
                var adjacent = adjacentAdd + pos;
                // Dont repeat draws
                if (visited.Contains(adjacent))
                {
                    continue;
                }
                
                if (tilemap.HasTile(adjacent))
                {
                    toVisit.Enqueue(adjacent);
                }

            }
        }
    }
    public void DrawHighlights(List<Vector3Int> squareCoords, EntityType type)
    {
        var currentHighlights = entityToHighlights[type];
        foreach (var coord in squareCoords)
        {
            if (currentHighlights.ContainsKey(coord))
            {
                currentHighlights[coord].SetActive(true);
            }
        }

    }
 
    public void ResetHighlights(List<Vector3Int> squareCoords, EntityType type)
    {
        var currentHighlights = entityToHighlights[type];
        foreach (var coord in squareCoords)
        {
            if (currentHighlights.ContainsKey(coord))
            {
                currentHighlights[coord].SetActive(false);
            }        }
    }
    
}
