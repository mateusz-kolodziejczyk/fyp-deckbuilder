using System.Collections.Generic;
using Card;
using Enums;
using Movement;
using Statics;
using TMPro;
using UnityEngine;

namespace Character
{
    public class PlayerTurn : MonoBehaviour
    {
        private TurnManagement turnManager;


        [SerializeField] private TextMeshProUGUI healthText, movementText, resourceText;
        private CardPlaying cardPlaying;
        private CharacterDataMono characterDataMono;
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

        public bool FinishedTurnSetup
        {
            get => finishedTurnSetup;
            private set => finishedTurnSetup = value;
        }

        private PlayerInput input;

        // Start is called before the first frame update
        void Start()
        {
            cardPlaying = GetComponent<CardPlaying>();
            characterDataMono = GetComponent<CharacterDataMono>();
            playerMovement = GetComponent<PlayerMovement>();
            cardTarget = GetComponent<CardTarget>();
            
            input = GetComponent<PlayerInput>();
            
            turnManager = GameObject.FindWithTag("TurnManager").GetComponent<TurnManagement>();
            if (turnManager == null)
            {
                Debug.Log("No Turn Manager Found");
            }

        }

        // Update is called once per frame
        void Update()
        {


        }

        public void SetUpTurn()
        {
            // Update Text
            UpdateText();
            
            cardPlaying.DrawCards();
            cardPlaying.DeselectCard();
            characterDataMono.ResourceAmount = characterDataMono.MAXResource;
            characterDataMono.ResetMovementPoints();
            playerMovement.CleanupMovementRange();
            cardTarget.ClearTargetSquares();
            finishedTurnSetup = true;
        }

        public void HandleTurn(List<Vector3Int> enemyPositions)
        {
            // Get input and update state at the same time
            state = input.HandleInput(state, enemyPositions);
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
                    playerMovement.ShowMovementRange(enemyPositions);
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

        }
        
        public void FinishTurn()
        {
            finishedTurnSetup = false;
            turnManager.FinishPlayerTurn();
            UpdateText();
            state = PlayerState.Idle;
        }

        public bool IsPlayerTurn()
        {
            return turnManager.CurrentTurn == Turn.Player;
        }

        private void UpdateText()
        {
            var s = $"{characterDataMono.HitPoints}/{characterDataMono.MAXHitPoints}";
            
            if (characterDataMono.TemporaryHitPoints > 0)
            {
                s += $" + {characterDataMono.TemporaryHitPoints}";
            }

            healthText.text = s;
            
            resourceText.text = $"{characterDataMono.ResourceAmount}/{characterDataMono.MAXResource}";
            movementText.text = $"{characterDataMono.MovementPoints}/{characterDataMono.MovementSpeed}";
        }
    }
}
