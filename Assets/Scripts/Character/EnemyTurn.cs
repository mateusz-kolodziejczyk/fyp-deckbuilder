using System.Collections;
using System.Collections.Generic;
using Enums;
using Movement;
using TMPro;
using UnityEngine;

[RequireComponent(typeof(EnemyMovement))]
[RequireComponent(typeof(Intent))]
public class EnemyTurn : MonoBehaviour
{
    private EnemyMovement enemyMovement;
    private Intent intent;
    private TurnManagement turnManager;
    [SerializeField] 
    private TextMeshProUGUI turnIndicator;

    private EnemyAttack enemyAttack;

    private bool attacked = false;
    // Start is called before the first frame update
    void Start()
    {
        enemyMovement = GetComponent<EnemyMovement>();
        intent = GetComponent<Intent>();
        turnManager = GameObject.FindWithTag("TurnManager").GetComponent<TurnManagement>();
        if (turnManager == null)
        {
            Debug.Log("No Turn Manager Found");
            return;
        }
        SetTurnText();
        enemyAttack = GetComponent<EnemyAttack>();
    }

    // Update is called once per frame
    void Update()
    {
        if (turnManager.CurrentTurn == Turn.Enemy)
        {
            attacked = false;
            intent.ClearIntent();
            enemyMovement.Move();
            enemyAttack.CalculateSquaresToAttack();
            intent.DrawIntent();
            turnManager.AdvanceTurn();
            SetTurnText();
        }

        // Attack only once at the start of the player's turn.
        if (!attacked && turnManager.CurrentTurn == Turn.Player)
        {
            enemyAttack.Attack();
            attacked = true;
        }
    }

    private void SetTurnText()
    {
        turnIndicator.text = $"Turn: {turnManager.CurrentTurn}";
    }
}
