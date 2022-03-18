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

        private CardTarget cardTarget;
        // This controls things that happen at the start of the player turn(draw cards, replenish energy)
        private bool finishedTurnSetup = false;

        private PlayerState state = PlayerState.Idle;
        
        public PlayerState State
        {
            get => state;
            set => state = value;
        }

        private PlayerInput input;

        // Start is called before the first frame update
        void Start()
        {
            cardPlaying = GetComponent<CardPlaying>();
            characterData = GetComponent<CharacterData>();
            playerMovement = GetComponent<PlayerMovement>();
            cardTarget = GetComponent<CardTarget>();
            
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
            // If not targeting, clear the target squares
            if (state != PlayerState.Targeting)
            {


            }
            // If not moving, clear movement range
            if (state != PlayerState.Moving)
            {
            }
            
            // Depending on the state of the player, either draw movement or current card
            switch (state)
            {
                case PlayerState.Moving:
                    playerMovement.ShowMovementRange();
                    cardTarget.ClearTargetSquares();
                    cardPlaying.DeselectCard();
                    break;
                case PlayerState.Targeting:
                    playerMovement.CleanupMovementRange();
                    cardTarget.HighlightTargetSquares();
                    break;
                case PlayerState.EndTurn:
                    playerMovement.CleanupMovementRange();
                    cardTarget.ClearTargetSquares();
                    cardPlaying.DeselectCard();
                    FinishTurn();
                    break;
                default:
                    break;
            }

            if (finishedTurnSetup)
            {
                return;
            }
            
            cardPlaying.DrawCards();
            cardPlaying.DeselectCard();
            characterData.ResourceAmount = characterData.MAXResource;
            characterData.ResetMovementPoints();
            playerMovement.CleanupMovementRange();
            cardTarget.ClearTargetSquares();
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
