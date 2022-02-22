using System.Collections;
using System.Collections.Generic;
using Enums;
using Movement;
using TMPro;
using UnityEngine;

[RequireComponent(typeof(EnemyMovement))]
public class EnemyTurn : MonoBehaviour
{
    private EnemyMovement enemyMovement;
    private TurnManagement turnManager;
    [SerializeField] 
    private TextMeshProUGUI turnIndicator;
    // Start is called before the first frame update
    void Start()
    {
        enemyMovement = GetComponent<EnemyMovement>();
        turnManager = GameObject.FindWithTag("TurnManager").GetComponent<TurnManagement>();
        if (turnManager == null)
        {
            Debug.Log("No Turn Manager Found");
            return;
        }
        SetTurnText();
    }

    // Update is called once per frame
    void Update()
    {
        if (turnManager.CurrentTurn == Turn.Enemy)
        {
            enemyMovement.Move(new Vector3Int(1,0));
            turnManager.AdvanceTurn();
            SetTurnText();
        }
    }

    private void SetTurnText()
    {
        turnIndicator.text = $"Turn: {turnManager.CurrentTurn}";
    }
}
