using System.Collections.Generic;
using ScriptableObjects;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace RewardScreen
{
    public class RewardCardManagement : MonoBehaviour
    {
        private int selectedCardIndex = -1;
        private List<GameObject> cardGameObjects = new();
        private List<CardScriptableObject> cardTypes = new();
        private PossibleRewardCards possibleRewardCards;

        public bool CardAlreadyChosen { get; set; } = false;
        public List<CardScriptableObject> CardTypes
        {
            get => cardTypes;
            private set => cardTypes = value;
        }

        public int SelectedCardIndex
        {
            get => selectedCardIndex;
            set => selectedCardIndex = value;
        }

        // Start is called before the first frame update
        private void Start()
        {
            possibleRewardCards = GameObject.FindWithTag("RewardCardManager").GetComponent<PossibleRewardCards>();
            foreach (Transform childTransform in gameObject.transform)
            {
                var child = childTransform.gameObject;
                cardGameObjects.Add(child);
                // Change the values on card to a specific scriptable card type
                var cardType = possibleRewardCards.Cards[Random.Range(0, possibleRewardCards.Cards.Count)];
                cardTypes.Add(cardType);

                // Update the info on the card

                var cardName = child.transform.GetChild(2).gameObject.GetComponent<TextMeshProUGUI>();
                var description = child.transform.GetChild(3).gameObject.GetComponent<TextMeshProUGUI>();
                var resourceCost = child.transform.GetChild(4).GetChild(1).GetComponent<TextMeshProUGUI>();

                // Make sure the card contains both a name and description
                if (cardName == null || description == null)
                {
                    continue;
                }

                cardName.text = cardType.prefabName;
                description.text = cardType.description;
                resourceCost.text = cardType.resourceCost.ToString();
            }
        }

        // Update is called once per frame
        void Update()
        {
        }

        public void SelectCard(int index)
        {
            // Do not run if reward was already selected
            if (CardAlreadyChosen)
            {
                return;
            }
            UnhighlightCards();

            // Unselect card
            if (index == selectedCardIndex)
            {
                selectedCardIndex = -1;
                return;
            }

            selectedCardIndex = index;
            HighlightCard(index);
        }

        public void HighlightCard(int index)
        {
            var card = cardGameObjects[index];
            if (card.activeSelf)
            {
                var cardHighlight = card.transform.GetChild(1).gameObject.GetComponent<Image>();
                cardHighlight.color =
                    new Color(cardHighlight.color.r, cardHighlight.color.g, cardHighlight.color.b, 0.5f);
            }
        }

        public void UnhighlightCards()
        {
            for (int i = 0; i < cardGameObjects.Count; i++)
            {
                var card = cardGameObjects[i];
                if (card.activeSelf)
                {
                    var cardHighlight = card.transform.GetChild(1).gameObject.GetComponent<Image>();
                    cardHighlight.color =
                        new Color(cardHighlight.color.r, cardHighlight.color.g, cardHighlight.color.b, 0f);
                }
            }
        }
    
    }
}