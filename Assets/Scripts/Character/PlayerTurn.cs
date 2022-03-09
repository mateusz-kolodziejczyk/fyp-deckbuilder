using Card;
using Enums;
using Movement;
using TMPro;
using UnityEngine;

namespace Character
{
    public class PlayerTurn : MonoBehaviour
    {
        private TurnManagement turnManager;

        [SerializeField] 
        private TextMeshProUGUI turnIndicator;

        private CardPlaying cardPlaying;
        private CharacterData characterData;
        private PlayerMovement playerMovement;

        // This controls things that happen at the start of the player turn(draw cards, replenish energy)
        private bool finishedTurnSetup = false;

        // Start is called before the first frame update
        void Start()
        {
            cardPlaying = GetComponent<CardPlaying>();
            characterData = GetComponent<CharacterData>();
            playerMovement = GetComponent<PlayerMovement>();
            
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
            if (finishedTurnSetup || turnManager.CurrentTurn != Turn.Player)
            {
                return;
            }
            
            cardPlaying.DrawCards();
            characterData.ResourceAmount = characterData.MAXResource;
            characterData.ResetMovementPoints();
            playerMovement.CleanupMovementRange();
            playerMovement.ShowMovementRange();
            finishedTurnSetup = true;
            
            Debug.Log(characterData.HitPoints);
        }

        public void FinishTurn()
        {
            finishedTurnSetup = false;
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
