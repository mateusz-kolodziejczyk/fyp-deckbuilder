using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Card;
using Unity.VisualScripting;
using UnityEngine;

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

        // Make sure that the index exists in the list
        if (!rewardCardManagement.CardTypes.ElementAtOrDefault(rewardCardManagement.SelectedCardIndex))
        {
            return;
        }

        var card = rewardCardManagement.CardTypes[rewardCardManagement.SelectedCardIndex];
        // Add card to player's deck.
        var player = GameObject.FindWithTag("Player");

        if (player != null && player.TryGetComponent(out Deck deck))
        {
            deck.Cards.Add(card);
        }

        rewardCardManagement.CardAlreadyChosen = true;
        rewardCardManagement.UnhighlightCards();
        rewardCardManagement.SelectedCardIndex = -1;
    }
}
