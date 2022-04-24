using System;
using System.Collections;
using System.Collections.Generic;
using Card;
using Enums;
using Helper;
using Movement;
using UnityEngine;
using UnityEngine.Tilemaps;

public class PlayerInput : MonoBehaviour
{
    private PlayerMovement movement;
    private CardPlaying cardPlaying;

    private Tilemap tilemap;
    private void Start()
    {
        movement = GetComponent<PlayerMovement>();
        cardPlaying = GetComponent<CardPlaying>();

        tilemap = GameObject.FindWithTag("TileMap").GetComponent<Tilemap>();
    }

    // Returns a state since a keyboard input might involve a change in state.
    public PlayerState HandleInput(PlayerState state, List<Vector3Int> enemyPositions)
    {
        if (Input.GetMouseButtonDown(0))
        {
            var (pos, tileExists) = GridHelper.MousePosToGrid(tilemap);
            if (tileExists)
            {
                switch (state)
                {
                    case PlayerState.Moving:
                        if (!enemyPositions.Contains(pos))
                        {
                            movement.UpdateCurrentCellMouse(pos);
                        }
                        break;
                    case PlayerState.Targeting:
                        if (cardPlaying.PlayCard(pos))
                        {
                            state = PlayerState.Idle;
                        }
                        break;
                    default:
                        break;
                } 
            }
        }
        
        // Switch between moving/idle explicitly
        if (Input.GetKeyDown(KeyCode.F))
        {
            // Switch between the states.
            state = state == PlayerState.Moving ? PlayerState.Idle : PlayerState.Moving;
            // Cleanup movement range
            movement.CleanupMovementRange();
        }
        
        // End turn
        if (Input.GetKeyDown(KeyCode.Space))
        {
            state = PlayerState.EndTurn;
        }
        return state;
    }
}
