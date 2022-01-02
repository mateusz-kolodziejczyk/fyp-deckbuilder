using System.Collections.Generic;
using Card;
using ScriptableObjects;
using TMPro;
using UnityEngine;

namespace UI
{
    public class DeckDrawer : MonoBehaviour
    {
        private List<GameObject> _cards = new ();

        [SerializeField]
        private Deck playerDeck;

        [SerializeField] private CardPlaying cardPlayer;



        public Deck PlayerDeck
        {
            get => playerDeck;
            set => playerDeck = value;
        }
        public CardPlaying CardPlayer
        {
            get => cardPlayer;
            set => cardPlayer = value;
        }
        // Start is called before the first frame update
        private void Start()
        {
            foreach (Transform childTransform in gameObject.transform)
            {
                var child = childTransform.gameObject;
                _cards.Add(child);
                child.SetActive(false);
            }
        }

        public void UpdateCards(List<CardScriptableObject> newCards)
        {
            for (int i = 0; i < _cards.Count; i++)
            {

                var card = _cards[i];
                // Set card to active so that children can be gotten.
                card.SetActive(true);

                var cardName = card.transform.GetChild(1).gameObject.GetComponent<TextMeshProUGUI>();
                var description= card.transform.GetChild(2).gameObject.GetComponent<TextMeshProUGUI>();
                // Make sure the card contains both a name and description
                if (cardName == null || description == null)
                {
                    continue;
                }
                
                // If the card does not exist in the players deck, deactivate it so that the player can't see it.
                if (i >= newCards.Count)
                {
                    card.SetActive(false);
                    continue;
                }
                
                cardName.text = newCards[i].prefabName;
                description.text = newCards[i].description;
            }
        }
    }
}
