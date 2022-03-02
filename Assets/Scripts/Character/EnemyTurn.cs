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
    }

    // Update is called once per frame
    void Update()
    {
        if (turnManager.CurrentTurn == Turn.Enemy)
        {
            intent.ClearIntent();
            enemyMovement.Move(new Vector3Int(1,0));
            intent.DrawIntent();
            turnManager.AdvanceTurn();
            SetTurnText();
        }
    }

    private void SetTurnText()
    {
        turnIndicator.text = $"Turn: {turnManager.CurrentTurn}";
    }
}
