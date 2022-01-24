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
    public class CardPlaying : MonoBehaviour
    {
        private Deck _deck;

        [NotNull] private List<CardScriptableObject> _hand = new ();

        [SerializeField] 
        private Health enemyHealth;
            
        [SerializeField]
        private GameObject uiDeck;

        private DeckDrawer _deckDrawer;


        // Start is called before the first frame update
        private void Start()
        {
            _deck = GetComponent<Deck>();
            _deckDrawer = uiDeck.GetComponent<DeckDrawer>();
        }

        // Update is called once per frame
        private void Update()
        {
            if (_deckDrawer != null)
            {
                _deckDrawer.UpdateCards(_hand);
            }
            
            if (Input.GetKeyDown(KeyCode.D))
            {
                DrawCards();
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
                if (_hand.Count >= 5)
                {
                    return;
                }
                
                var c = _deck.DrawCard();

                // Drawing a card might result in a null
                // TODO Reshuffle used cards into the deck after all cards are exhausted.
                if (c != null)
                {
                    _hand.Add(c);
                }
            }
        }

        public bool PlayCard(int index)
        {
            if (index < 0 || index >= _hand.Count)
            {
                return false;
            }

            var c = _hand[index];
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
            _hand.RemoveAt(index);
            return false;
        }
    }
}
