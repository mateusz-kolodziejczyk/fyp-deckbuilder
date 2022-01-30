using Enums;
using TMPro;
using UnityEngine;

namespace Character
{
    public class PlayerTurn : MonoBehaviour
    {
        private TurnManagement turnManager;

        [SerializeField] 
        private TextMeshProUGUI turnIndicator;

        // Start is called before the first frame update
        void Start()
        {
            turnManager = GameObject.FindWithTag("TurnManager").GetComponent<TurnManagement>();
            if (turnManager == null)
            {
                Debug.Log("No Turn Manager Found");
                return;
            }
            turnIndicator.text = $"Turn: {turnManager.CurrentTurn}";

        }

        // Update is called once per frame
        void Update()
        {
        
        }

        public void FinishTurn()
        {
            turnManager.FinishPlayerTurn();
            updateText();
        }

        public bool IsPlayerTurn()
        {
            return turnManager.CurrentTurn == Turn.Player;
        }

        private void updateText()
        {
            turnIndicator.text = $"Turn: {turnManager.CurrentTurn}";
        }
    }
}
