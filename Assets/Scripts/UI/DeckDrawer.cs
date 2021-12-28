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
            foreach (GameObject child in gameObject.transform)
            {
                cards.Add(child);
                child.SetActive(false);
            }
        }

        public void UpdateCards(List<CardScriptableObject> newCards)
        {
            for (int i = 0; i < cards.Count; i++)
            {
                var card = cards[i];
                
                var cardName = card.transform.GetChild(1).GetComponent<TextMeshPro>();
                var description= card.transform.GetChild(2).GetComponent<TextMeshPro>();
                
                // If the card does not exist in the players deck, deactivate it so that the player can't see it.
                if (i < newCards.Count)
                {
                    card.SetActive(false);
                    continue;
                }
                
                cardName.text = newCards[i].prefabName;
                description.text = newCards[i].prefabName;
                card.SetActive(true);
            }
        }
    }
}
