using System;
using System.Collections.Generic;
using System.Linq;
using Character;
using Enums;
using JetBrains.Annotations;
using ScriptableObjects;
using UI;
using UnityEditor;
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

        [SerializeField] 
        private Health enemyHealth;
            
        [SerializeField]
        private GameObject uiDeck;

        private DeckDrawer deckDrawer;

        private PlayerTurn playerTurn;

        private CharacterData characterData;

        private Resource resource;

        // Start is called before the first frame update
        private void Start()
        {
            playerTurn = GetComponent<PlayerTurn>();
            
            deck = GetComponent<Deck>();
            deckDrawer = uiDeck.GetComponent<DeckDrawer>();
            characterData = GetComponent<CharacterData>();
            resource = GetComponent<Resource>();
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
            
            if (Input.GetKeyDown(KeyCode.D))
            {
                DrawCards();
            }

            if (Input.GetKeyDown(KeyCode.Space))
            {
                playerTurn.FinishTurn();
            }
        }

        // Modifier modifies the number of cards drawn.
        public void DrawCards(int modifier = 0)
        {
            var defaultNumDrawn = 5;
            // Do not draw anything if it would try to draw less than 0 cards.
            if (defaultNumDrawn + modifier < 0)
            {
                return;
            }

            foreach (var _ in Enumerable.Range(0, 5 + modifier))
            {
                // TODO Max hand size check, compare to number of cards in hand already
                // Set it to 5 for now, as only 5 cards are displayed
                if (hand.Count >= 5)
                {
                    return;
                }
                
                var c = deck.DrawCard();

                // Drawing a card might result in a null
                // TODO Reshuffle used cards into the deck after all cards are exhausted.
                if (c != null)
                {
                    hand.Add(c);
                }
            }
        }

        public bool PlayCard(int index)
        {
            // If it's not the player's turn, do not allow playing cards.
            if (!playerTurn.IsPlayerTurn())
            {
                return false;
            }
            
            if (index < 0 || index >= hand.Count)
            {
                return false;
            }

            var c = hand[index];
            // Make sure the player has enough resource to play the card
            if (!resource.HasEnoughResources(c.resourceCost))
            {
                return false;
            }
            // Otherwise remove the resource amount from the player equivalent to the cost of the card
            resource.UpdateResources(-c.resourceCost);
            // Update the resource cost text
            resource.UpdateText();
            // Play the card
            Debug.Log($"Card Played \n Name: {c.prefabName} Description: {c.description}");

            var playerHealth = GetComponent<Health>();
            if (c is SimpleCardScriptableObject card)
            {
                switch (card.type)
                {
                    case CardType.Attack:
                        enemyHealth.UpdateHealth(-card.magnitude);
                        enemyHealth.UpdateHealthText();
                        break;
                    case CardType.Defence:
                        playerHealth.AddTemporaryHP(card.magnitude);
                        playerHealth.UpdateHealthText();
                        break;
                    default:
                        break;
                }
            }
            hand.RemoveAt(index);
            return true;
        }
    }
}
