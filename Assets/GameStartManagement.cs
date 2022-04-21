using System.Collections;
using System.Collections.Generic;
using Card;
using Character;
using ScriptableObjects;
using Statics;
using UnityEngine;

public class GameStartManagement : MonoBehaviour
{
    [SerializeField] private PlayerScriptableObject playerCharacter;

    public void UpdatePlayerData()
    {
        // Do not update playre data if it was already added
        if (TryGetComponent(out CharacterDataMono _))
        {
            return;
        }

        var playerData = new CharacterData(Vector3Int.zero, playerCharacter.movementPoints,
            playerCharacter.startResource, playerCharacter.hp, playerCharacter.startCurrency);

        PlayerDataStore.CharacterData = playerData;
        PlayerDataStore.Deck = playerCharacter.startDeck;
    }
}
