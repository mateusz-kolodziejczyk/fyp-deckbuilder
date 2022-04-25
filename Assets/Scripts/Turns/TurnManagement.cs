using System.Collections.Generic;
using System.Linq;
using Character;
using Enums;
using UnityEngine;

namespace Turns
{
    public class TurnManagement : MonoBehaviour
    {
        private Turn currentTurn;

        private GameManager gameManager;
        public Turn CurrentTurn
        {
            get => currentTurn;
            set => currentTurn = value;
        }

        // Get these components in a lazy fashion, as they might not be found at the start
        private List<EnemyTurn> enemyTurnComponents = new ();

        private PlayerTurn playerTurnComponent;

        private bool foundComponents;

        private List<Vector3Int> enemyPositions;

        private DrawSquares drawSquares;
    
        // Start is called before the first frame update
        private void Start()
        {
            drawSquares = GameObject.FindWithTag("GridDrawerController").GetComponent<DrawSquares>();
            currentTurn = Turn.Neutral;
            gameManager = GameObject.FindWithTag("GameController").GetComponent<GameManager>();
        }

        // Update is called once per frame
        private void Update()
        {
            if(!foundComponents)
            {
                GetTurnComponents();
            }
        
            // If enemy positions are null, get them
            enemyPositions ??= gameManager.GetEnemyPositions();
        
            switch (CurrentTurn)
            {
                // Turn manager handles both player and Enemy turns
                case Turn.Neutral:
                    CurrentTurn = Turn.Player;
                    // Update the health texts of the enemies
                    foreach (var o in gameManager.Enemies.Where(x => x.activeSelf))
                    {
                        o.GetComponent<EnemyHealth>().UpdateHealthText();
                    }

                    break;
                case Turn.Enemy:
                {
                    // Clear all the targeting highlights from last turn.
                    drawSquares.ResetHighlights(HighlightType.EnemyAttack);
                
                    foreach (var enemyTurnComponent in enemyTurnComponents.Where(enemyTurnComponent => enemyTurnComponent.gameObject.activeSelf))
                    {
                        enemyTurnComponent.MakeTurn(enemyPositions);
                    }
                    AdvanceTurn();
                    // Get the enemy positions right after the enemies finish their turn
                    enemyPositions = gameManager.GetEnemyPositions();
                    break;
                }
                case Turn.Player:
                {
                    if (!playerTurnComponent.FinishedTurnSetup)
                    {
                        playerTurnComponent.SetUpTurn();
                    }
                    playerTurnComponent.HandleTurn(enemyPositions);
                    break;
                }
                default:
                    break;
            }
        }

        private void GetTurnComponents()
        {
            if (gameManager.Player == null) return;
            if (gameManager.Player.TryGetComponent(out PlayerTurn playerTurn))
            {
                playerTurnComponent = playerTurn;
            }
            else
            {
                return;
            }

            if (gameManager.Enemies == null) return;
        
            foreach (var o in gameManager.Enemies)
            {
                if (o.TryGetComponent(out EnemyTurn enemyTurn))
                {
                    enemyTurnComponents.Add(enemyTurn);
                }
            }

            foundComponents = true;
        }
        public void AdvanceTurn()
        {
            if (currentTurn == Turn.Player)
            {
                currentTurn = Turn.Enemy;
            }
            else
            {
                currentTurn = Turn.Player;
            }
            gameManager.CheckStatus();
        }

        public void FinishPlayerTurn()
        {
            if (currentTurn != Turn.Player) return;
        
            currentTurn = Turn.Enemy;
            playerTurnComponent.FinishTurn();
        
        }
        public void FinishEnemyTurn()
        {
            if (currentTurn == Turn.Enemy)
            {
                currentTurn = Turn.Player;
            }
        }

        // Reset the turn to return to a neutral state
        public void ResetTurn()
        {
            currentTurn = Turn.Neutral;
        }

        public void ReHightlighSquares()
        {
            drawSquares.ResetHighlights(HighlightType.EnemyAttack);

            foreach (var o in gameManager.Enemies.Where(x => x.activeSelf))
            {
                if (!o.TryGetComponent(out Intent intent)) return;
            
                intent.DrawIntent();
            }
        }
    }
}
