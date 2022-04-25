using Turns;
using UnityEngine;

namespace Character
{
    // This handles all aspects of an enemy dying
    public class EnemyDeath : MonoBehaviour
    {
        private TurnManagement turnManagement;
        private EnemyAttack attack;

        private void Start()
        {
            turnManagement = GameObject.FindWithTag("TurnManager").GetComponent<TurnManagement>();
            attack = GetComponent<EnemyAttack>();
        }

        public void Die()
        {
            attack.SquaresToAttack = new();
            turnManagement.ReHightlighSquares();
            gameObject.SetActive(false);
        }
    }
}