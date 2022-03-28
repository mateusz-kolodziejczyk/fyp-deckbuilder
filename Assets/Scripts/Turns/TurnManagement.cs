using System.Collections;
using System.Collections.Generic;
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

    // Start is called before the first frame update
    void Start()
    {
        currentTurn = Turn.Player;
        gameManager = GameObject.FindWithTag("GameController").GetComponent<GameManager>();
    }

    // Update is called once per frame
    void Update()
    {
        
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
