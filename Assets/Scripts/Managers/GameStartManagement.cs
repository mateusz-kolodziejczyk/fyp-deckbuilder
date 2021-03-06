using System.Linq;
using Character;
using ScriptableObjects;
using Statics;
using UnityEngine;

namespace Managers
{
    public class GameStartManagement : MonoBehaviour
    {
        [SerializeField] private PlayerScriptableObject playerCharacter;

        public void ResetData()
        {
            PlayerDataStore.ResetData();
            CampaignMapDataStore.ResetData();
        }
        public void UpdatePlayerData()
        {
            var playerData = new CharacterData(Vector3Int.zero, playerCharacter.movementPoints,
                playerCharacter.startResource, playerCharacter.hp, playerCharacter.startCurrency);

            PlayerDataStore.CharacterData = playerData;
            PlayerDataStore.Deck = playerCharacter.startDeck.Select(x => x).ToList();
        }
    }
}
