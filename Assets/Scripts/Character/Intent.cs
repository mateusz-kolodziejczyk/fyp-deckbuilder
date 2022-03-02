using System;
using System.Collections;
using System.Collections.Generic;
using Character;
using Helper;
using UnityEngine;
using UnityEngine.Tilemaps;

public class Intent : MonoBehaviour
{
    private Tilemap tilemap;
    private EnemyData data;
    private List<Vector3Int> paintedTiles;
    private void Start()
    {
        tilemap = GameObject.FindWithTag("TileMap").GetComponent<Tilemap>();
        data = GetComponent<EnemyData>();
        paintedTiles = new List<Vector3Int>();
    }

    public void DrawIntent()
    {
        var squaresToPaint = new List<Vector3Int>();
        // NOTE: For now it directly picks the first ability from the ability list, in the future it should
        // Use the ability chooser class to find which ability was chosen for the turn.
        if (data.Abilities.Count > 0)
        {
            var ability = data.Abilities[0];
            var directions = new List<Vector3Int> {Vector3Int.up, Vector3Int.down, Vector3Int.left, Vector3Int.right};
            foreach (var direction in directions)
            {
                for (int i = 1; i <= ability.range; i++)
                {
                    squaresToPaint.Add(i*direction + data.Position);
                }
            }
        }
        
        GridPainter.PaintSquares(ref tilemap, squaresToPaint, Color.gray);
        paintedTiles = squaresToPaint;

    }

    public void ClearIntent()
    {
        GridPainter.ResetSquares(ref tilemap, paintedTiles);
        paintedTiles.Clear();
    }
}
