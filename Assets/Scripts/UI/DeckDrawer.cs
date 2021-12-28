using System.Collections.Generic;
using ScriptableObjects;
using TMPro;
using UnityEngine;

namespace UI
{
    public class DeckDrawer : MonoBehaviour
    {
        private List<GameObject> cards = new List<GameObject>();
        // Start is called before the first frame update
        private void Start()
        {
            foreach (Transform childTransform in gameObject.transform)
            {
                var child = childTransform.gameObject;
                cards.Add(child);
                child.SetActive(false);
            }
        }

        public void UpdateCards(List<CardScriptableObject> newCards)
        {
            for (int i = 0; i < cards.Count; i++)
            {

                var card = cards[i];
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
