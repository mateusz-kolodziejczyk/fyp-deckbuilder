using System.Collections.Generic;
using ScriptableObjects;
using Statics;
using TMPro;
using UnityEngine;
using Random = UnityEngine.Random;

public class ManageShop : MonoBehaviour
{
    [SerializeField] private List<CardScriptableObject> possibleCards = new();
    
    public List<CardScriptableObject> PossibleCards 
    {
        get => possibleCards;
        set => possibleCards = value;
    }

    [SerializeField] private TextMeshProUGUI currencyText;
    private List<ShopCardItem> shopCardItems = new();

    private void Start()
    {
        var shopCardObjectArray = GameObject.FindGameObjectsWithTag("ShopCardItem");
        foreach (var o in shopCardObjectArray)
        {
            shopCardItems.Add(o.GetComponent<ShopCardItem>());
        }
        InitialiseShopCards();
        SetCurrencyText();
    }

    private void InitialiseShopCards()
    {
        foreach (var item in shopCardItems)
        {
            var cardType = possibleCards[Random.Range(0, possibleCards.Count)];
            item.Card = cardType;
            item.SetFields();
        }
    }
    public bool BuyCard(CardScriptableObject card)
    {
        // Check if player has enough money
        if (PlayerDataStore.CharacterData.Currency < card.goldValue)
        {
            return false;
        }
        
        PlayerDataStore.Deck.Add(card);
        PlayerDataStore.CharacterData.Currency -= card.goldValue;
        SetCurrencyText();
        return true;
    }

    private void SetCurrencyText()
    {
        if (currencyText == null)
        {
            return;
        }
        currencyText.text = $"Currency: {PlayerDataStore.CharacterData.Currency}";
    }
}
