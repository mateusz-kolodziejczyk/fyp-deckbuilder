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

        private PlayerState state = PlayerState.Idle;

        private PlayerInput input;

        // Start is called before the first frame update
        void Start()
        {
            cardPlaying = GetComponent<CardPlaying>();
            characterData = GetComponent<CharacterData>();
            playerMovement = GetComponent<PlayerMovement>();
            input = GetComponent<PlayerInput>();
            
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

            
            if (turnManager.CurrentTurn != Turn.Player)
            {
                return;
            }
            // Get input and update state at the same time
            state = input.HandleInput(state);
            
            // Depending on the state of the player, either draw movement or current card
            switch (state)
            {
                case PlayerState.Moving:
                    playerMovement.ShowMovementRange();
                    break;
                case PlayerState.Targeting:
                    break;
                case PlayerState.EndTurn:
                    FinishTurn();
                    break;
                default:
                    break;
            }
            
            Debug.Log(state);
            if (finishedTurnSetup)
            {
                return;
            }
            
            cardPlaying.DrawCards();
            characterData.ResourceAmount = characterData.MAXResource;
            characterData.ResetMovementPoints();
            playerMovement.CleanupMovementRange();
            finishedTurnSetup = true;
            
            Debug.Log(characterData.HitPoints);
        }

        public void FinishTurn()
        {
            finishedTurnSetup = false;
            turnManager.FinishPlayerTurn();
            updateText();
            state = PlayerState.Idle;
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
