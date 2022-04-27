using System.Linq;
using Managers;
using Statics;
using UnityEngine;

namespace RewardScreen
{
    public class CardSelectConfirmButton : MonoBehaviour
    {
        private RewardCardManagement rewardCardManagement;
        // Start is called before the first frame update
        void Start()
        {
            rewardCardManagement =
                GameObject.FindWithTag("RewardCards").GetComponent<RewardCardManagement>();
        }

        public void SelectReward()
        {
            // Do not do anything if the component is null
            if (rewardCardManagement == null)
            {
                return;
            }
            
            // If the index exists, the card exists
            // If it does, add it to the deck
            if (rewardCardManagement.CardTypes.ElementAtOrDefault(rewardCardManagement.SelectedCardIndex))
            {
                var card = rewardCardManagement.CardTypes[rewardCardManagement.SelectedCardIndex];
        
                // Add card to player's deck, by adding it to the stored data version
                PlayerDataStore.Deck.Add(card);

                rewardCardManagement.CardAlreadyChosen = true;
                rewardCardManagement.UnhighlightCards();
                rewardCardManagement.SelectedCardIndex = -1;
            }
            var gameManager = GameObject.FindWithTag("GameController").GetComponent<GameManager>();
            gameManager.UpdatePlayerData();
            gameManager.GoToCampaign();
        }
    }
}
