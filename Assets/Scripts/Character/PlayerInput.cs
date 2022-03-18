using System;
using System.Collections;
using System.Collections.Generic;
using Enums;
using Movement;
using UnityEngine;

public class PlayerInput : MonoBehaviour
{
    private PlayerMovement movement;
    private void Start()
    {
        movement = GetComponent<PlayerMovement>();
    }

    // Returns a state since a keyboard input might involve a change in state.
    public PlayerState HandleInput(PlayerState state)
    {
        if (Input.GetMouseButtonDown(0))
        {
            switch (state)
            {
                case PlayerState.Moving:
                    movement.UpdateCurrentCellMouse();
                    break;
                case PlayerState.Targeting:
                    break;
                default:
                    break;
            }
        }
        
        // Switch between moving/idle explicitly
        if (Input.GetKeyDown(KeyCode.F))
        {
            // Switch between the states.
            state = state == PlayerState.Idle ? PlayerState.Moving : PlayerState.Idle;
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
