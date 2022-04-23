using System;
using System.Collections;
using System.Collections.Generic;
using Character;
using Enums;
using UnityEngine;


public class TurnManagement : MonoBehaviour
{
    private Turn currentTurn;

    private GameManager gameManager;
    public Turn CurrentTurn
    {
        get => currentTurn;
        set => currentTurn = value;
    }

    // Get these components in a lazy fashion, as they might not be found at the start
    private List<EnemyTurn> enemyTurnComponents = new ();

    private PlayerTurn playerTurnComponent;

    private bool foundComponents;
    
    // Start is called before the first frame update
    private void Start()
    {
        currentTurn = Turn.Player;
        gameManager = GameObject.FindWithTag("GameController").GetComponent<GameManager>();
    }

    // Update is called once per frame
    private void Update()
    {
        switch (foundComponents)
        {
            case false:
                GetTurnComponents();
                break;
        }

        switch (CurrentTurn)
        {
            // Turn manager handles both player and Enemy turns
            case Turn.Neutral:
                CurrentTurn = Turn.Player;
                break;
            case Turn.Enemy:
            {
                foreach (var enemyTurnComponent in enemyTurnComponents)
                {
                    enemyTurnComponent.MakeTurn();
                }
                AdvanceTurn();
                break;
            }
            case Turn.Player:
            {
                if (!playerTurnComponent.FinishedTurnSetup)
                {
                    playerTurnComponent.SetUpTurn();
                }
                playerTurnComponent.HandleTurn();
                break;
            }
            default:
                break;
        }
    }

    private void GetTurnComponents()
    {
        if (gameManager.Player == null) return;
        if (gameManager.Player.TryGetComponent(out PlayerTurn playerTurn))
        {
            playerTurnComponent = playerTurn;
        }
        else
        {
            return;
        }

        if (gameManager.Enemies == null) return;
        
        foreach (var o in gameManager.Enemies)
        {
            if (o.TryGetComponent(out EnemyTurn enemyTurn))
            {
                enemyTurnComponents.Add(enemyTurn);
            }
        }

        foundComponents = true;
    }
    public void AdvanceTurn()
    {
        if (currentTurn == Turn.Player)
        {
            currentTurn = Turn.Enemy;
        }
        else
        {
            currentTurn = Turn.Player;
        }
        gameManager.CheckStatus();
    }

    public void FinishPlayerTurn()
    {
        if (currentTurn == Turn.Player)
        {
            currentTurn = Turn.Enemy;
        }
    }

    public void FinishEnemyTurn()
    {
        if (currentTurn == Turn.Enemy)
        {
            currentTurn = Turn.Player;
        }
    }

    // Reset the turn to return to a neutral state
    public void ResetTurn()
    {
        currentTurn = Turn.Neutral;
    }
}
