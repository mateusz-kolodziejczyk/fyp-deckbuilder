using System;
using System.Collections.Generic;
using System.Linq;
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
            // TODO Max hand size check, compare to number of cards in hand already
            // Set it to 5 for now, as only 5 cards are displayed
            if (_hand.Count >= 5)
            {
                return;
            }
            foreach (var _ in Enumerable.Range(0, 5 + modifier))
            {
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
            _hand.RemoveAt(index);
            Debug.Log($"Card Played \n Name: {c.prefabName} Description: {c.description}");
            return false;
        }
    }
}
