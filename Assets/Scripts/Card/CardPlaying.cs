using System.Collections.Generic;
using System.Linq;
using Audio;
using Character;
using Enemy;
using Enums;
using JetBrains.Annotations;
using Managers;
using Player;
using ScriptableObjects;
using UI;
using UnityEngine;

namespace Card
{
    [RequireComponent(typeof(Deck))]
    [RequireComponent(typeof(PlayerTurn))]
    [RequireComponent(typeof(Resource))]
    public class CardPlaying : MonoBehaviour
    {
        private Deck deck;

        [NotNull] private List<CardScriptableObject> hand = new ();
        
            

        private DeckDrawer deckDrawer;

        private PlayerTurn playerTurn;

        private CharacterDataMono characterDataMono;

        private Resource resource;

        private int currentCardIndex = -1;

        private CardTarget cardTarget;


        private GameManager gameManager;

        private PlayerAudio playerAudio;

        // Start is called before the first frame update
        private void Start()
        {
            playerTurn = GetComponent<PlayerTurn>();
            cardTarget = GetComponent<CardTarget>();
            gameManager = GameObject.FindWithTag("GameController").GetComponent<GameManager>();
            deck = GetComponent<Deck>();
            deckDrawer = GameObject.FindWithTag("UIDeck").GetComponent<DeckDrawer>();
            characterDataMono = GetComponent<CharacterDataMono>();
            resource = GetComponent<Resource>();
            playerAudio = GetComponent<PlayerAudio>();
        }

        // Update is called once per frame
        private void Update()
        {
            if (!playerTurn.IsPlayerTurn())
            {
                return;
            }
            if (deckDrawer != null)
            {
                deckDrawer.UpdateCards(hand);
            }
        }

        // Modifier modifies the number of cards drawn.
        public void DrawCards(int modifier = 0)
        {
            var defaultNumDrawn = 3;
            // Do not draw anything if it would try to draw less than 0 cards.
            if (defaultNumDrawn + modifier < 0)
            {
                return;
            }

            foreach (var _ in Enumerable.Range(0, defaultNumDrawn + modifier))
            {
                // Set it to 5 for now, as only 5 cards are displayed
                if (hand.Count >= 5)
                {
                    return;
                }
                
                var c = deck.DrawCard();

                // Drawing a card might result in a null, if it does, reshuffle discards
                if (c != null)
                {
                    hand.Add(c);
                }
                else
                {
                    deck.ReshuffleDiscards();
                }
            }
        }

        public CardScriptableObject GetCurrentCard()
        {
            if (hand.ElementAtOrDefault(currentCardIndex) != null)
            {
                return hand[currentCardIndex];
            }

            return null;
        }

        public bool PlayCard(Vector3Int position)
        {
            if (hand.ElementAtOrDefault(currentCardIndex) == null)
            {
                return false;
            }
            
            var c = hand[currentCardIndex];
            
            // Check if position is currently drawn.
            if (!cardTarget.ContainsPos(position))
            {
                return false;
            }


            var playerHealth = GetComponent<Health>();
            if (c is SimpleCardScriptableObject card)
            {
                switch (card.type)
                {
                    case AbilityType.Attack:
                        var enemy = gameManager.GetEnemyAtPosition(position);
                        if (enemy != null)
                        {
                            playerAudio.PlayAudio();
                            if(enemy.TryGetComponent(out EnemyHealth enemyHealth))
                            {
                                enemyHealth.UpdateHealth(-card.magnitude);
                                enemyHealth.UpdateHealthText();
                                if (!enemyHealth.IsAlive() && enemy.TryGetComponent(out EnemyDeath enemyDeath))
                                {
                                    enemyDeath.Die();
                                    gameManager.CheckStatus();
                                }
                            }
                        }
                        else
                        {
                            return false;
                        }
                        break;
                    case AbilityType.Defence:
                        playerHealth.AddTemporaryHP(card.magnitude);
                        playerHealth.UpdateHealthText();
                        break;
                    case AbilityType.Special:
                        break;
                    default:
                        break;
                }
            }
            
            // Only update resources if card was successfully played
            resource.UpdateResources(-c.resourceCost);
            // Update the resource cost text
            resource.UpdateText();
            // Play the card
            Debug.Log($"Card Played \n Name: {c.prefabName} Description: {c.description}");

            
            hand.RemoveAt(currentCardIndex);
            
            cardTarget.ClearTargetSquares();
            // Put card in discard pile
            deck.DiscardPile.Add(c);
            
            // Deselect card
            DeselectCard();
            
            return true;
        }

        public void DeselectCard()
        {
            if (currentCardIndex != -1)
            {
                currentCardIndex = -1;
                deckDrawer.UnhighlightCards();
            }
        }
        
        public void SelectCard(int index)
        {
            // If it's not the player's turn, do not allow playing cards.
            if (!playerTurn.IsPlayerTurn())
            {
                return;
            }
            // If index is teh same as the current index, deselect card
            if (index == currentCardIndex)
            {
                DeselectCard();
                playerTurn.State = PlayerState.Idle;
                cardTarget.ClearTargetSquares();
                return;
            }
            if (index < 0 || index >= hand.Count)
            {
                return;
            }

            var c = hand[index];
            // Make sure the player has enough resource to play the card
            if (!resource.HasEnoughResources(c.resourceCost))
            {
                return;
            }
            
                        
            
            // set the current card id to whatever was selected
            currentCardIndex = index;
            
            // Unhighlight other cards
            deckDrawer.UnhighlightCards();
            
            // Unhighlight currently highlighted squares
            cardTarget.ClearTargetSquares();
            

            
            // Highlight the card
            deckDrawer.HighlightCard(currentCardIndex);

            // If card has range 0, highlight the playerSquare
            if (c.range == 0)
            {
                cardTarget.HighlightPlayerSquare();
            }
            else
            {
                // Highlight new squares
                cardTarget.HighlightTargetSquares();  
            }
            
            // Set the playeturn state to "targeting"
            playerTurn.State = PlayerState.Targeting;
        }
    }
}
