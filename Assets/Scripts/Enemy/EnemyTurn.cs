using System.Collections.Generic;
using Character;
using Movement;
using Turns;
using UnityEngine;

namespace Enemy
{
    [RequireComponent(typeof(EnemyMovement))]
    [RequireComponent(typeof(Intent))]
    public class EnemyTurn : MonoBehaviour
    {
        private EnemyMovement enemyMovement;
        private Intent intent;
        private TurnManagement turnManager;

        private EnemyAttack enemyAttack;

        private Health health;
        private bool attacked = false;
    
        // Start is called before the first frame update
        void Start()
        {
            health = GetComponent<Health>();
            enemyMovement = GetComponent<EnemyMovement>();
            intent = GetComponent<Intent>();
            turnManager = GameObject.FindWithTag("TurnManager").GetComponent<TurnManagement>();
            if (turnManager == null)
            {
                Debug.Log("No Turn Manager Found");
                return;
            }
            enemyAttack = GetComponent<EnemyAttack>();
        }

        // Update is called once per frame
        void Update()
        {
        }

        public void MakeTurn(List<Vector3Int> enemyPositions)
        {
            if (health.IsAlive())
            {
                enemyAttack.Attack();
                enemyMovement.Move(enemyPositions);
                enemyAttack.CalculateSquaresToAttack();
                intent.DrawIntent();
            }
            else
            {
                // Deactivate if dead
                gameObject.SetActive(false);
            }
        }

    }
}
