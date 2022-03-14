using System;
using System.Collections;
using System.Collections.Generic;
using Character;
using Enums;
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
    private DrawSquares drawSquares;

    private void Start()
    {
        tilemap = GameObject.FindWithTag("TileMap").GetComponent<Tilemap>();
        data = GetComponent<EnemyData>();
        paintedTiles = new List<Vector3Int>();
        enemyAttack = GetComponent<EnemyAttack>();
        
        var gridDrawerController = GameObject.FindWithTag("GridDrawerController");
        if (gridDrawerController != null)
        {
            if (gridDrawerController.TryGetComponent(out DrawSquares drawSquares))
            {
                this.drawSquares = drawSquares;
            }
        }
    }

    public void DrawIntent()
    {
        var squaresToPaint = enemyAttack.SquaresToAttack;
        if (drawSquares != null)
        {
            drawSquares.DrawHighlights(squaresToPaint, EntityType.Enemy);
        }
        paintedTiles = squaresToPaint;
    }

    public void ClearIntent()
    {
        if (drawSquares != null)
        {
            drawSquares.ResetHighlights(paintedTiles, EntityType.Enemy);

        }
        paintedTiles.Clear();
    }
}
