using System;
using System.Collections;
using System.Collections.Generic;
using Character;
using Helper;
using UnityEngine;
using UnityEngine.Tilemaps;

[RequireComponent(typeof(EnemyAttack))]
public class Intent : MonoBehaviour
{
    private Tilemap tilemap;
    private EnemyData data;
    private List<Vector3Int> paintedTiles;
    private EnemyAttack enemyAttack;
    private void Start()
    {
        tilemap = GameObject.FindWithTag("TileMap").GetComponent<Tilemap>();
        data = GetComponent<EnemyData>();
        paintedTiles = new List<Vector3Int>();
        enemyAttack = GetComponent<EnemyAttack>();
    }

    public void DrawIntent()
    {
        var squaresToPaint = enemyAttack.SquaresToAttack;
        GridPainter.PaintSquares(ref tilemap, squaresToPaint, Color.gray);
        paintedTiles = squaresToPaint;

    }

    public void ClearIntent()
    {
        GridPainter.ResetSquares(ref tilemap, paintedTiles);
        paintedTiles.Clear();
    }
}
