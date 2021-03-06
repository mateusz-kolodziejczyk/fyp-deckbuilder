using System;
using System.Collections.Generic;
using System.Linq;
using Card;
using Character;
using Enemy;
using Movement;
using Player;
using SceneManagement;
using ScriptableObjects;
using Statics;
using UnityEngine;

namespace Managers
{
    public class GameManager : MonoBehaviour
    {
        private GameObject rewardScreen;
        private CharacterDataMono playerDataMono;

        public List<GameObject> Enemies { get; private set; }
        public GameObject Player { get; private set; }
        // Start is called before the first frame update

        private BattleScriptableObject currentEncounter;
        void Start()
        {
            FindPlayer();
            rewardScreen = GameObject.FindWithTag("RewardScreen");
            rewardScreen.SetActive(false);
            SetupPlayer();
            SetupEnemies();
        }

        // Update is called once per frame
        void Update()
        {
        
        }

        private void SetupPlayer()
        {
            var player = GameObject.FindWithTag("Player");
            Player = player;
            if (player.TryGetComponent(out Deck deck))
            {
                deck.Cards = PlayerDataStore.Deck.Select(x => x).ToList();
                deck.Shuffle();
                Debug.Log(deck.Cards.Count);
            }

            if (!player.TryGetComponent(out PlayerDataMono data)) return;
        
            // Player stats
            // If it doesn't already exist, return;
            var pd = PlayerDataStore.CharacterData;
            if (pd == null)
            {
                return;
            }
            
            // Load in data
            // HP
            data.HitPoints = pd.HitPoints;
            data.MAXHitPoints = pd.MAXHitPoints;
            
            // Resource
            data.ResourceAmount = pd.MAXResource;
            data.MAXResource = pd.MAXResource;
            
            // Movement Points
            data.MovementSpeed = pd.MovementSpeed;
            data.MovementPoints = pd.MovementSpeed;
            
            // Player Position
            if (!CampaignMapDataStore.EncounterScriptableObjects.TryGetValue(CampaignMapDataStore.CurrentSquare,
                out var encounterScriptableObject)) return;
            if (encounterScriptableObject is BattleScriptableObject encounter)
            {
                data.Position = encounter.playerStartPosition;
                currentEncounter = encounter;
            }
        
            // Move the player to the correct location
            if (player.TryGetComponent(out PlayerMovement playerMovement))
            {
                playerMovement.UpdatePositionToDataPosition();
            }

        }

        private void SetupEnemies()
        {
            if (currentEncounter == null)
            {
                // If current encounter is null, try to get the encounter manually.
                if (!CampaignMapDataStore.EncounterScriptableObjects.TryGetValue(CampaignMapDataStore.CurrentSquare,
                    out var encounterScriptableObject)) return;
                if (encounterScriptableObject is not BattleScriptableObject encounter) return;
                currentEncounter = encounter;
            }


            // Zip the enemy and enemy positions together as they have to be separated for the scritpable object
            var enemyObjectsAndPositions = currentEncounter.enemies.Zip(currentEncounter.enemyPositions, Tuple.Create);

            Enemies = new();
        
            foreach (var (enemyScriptableObject, pos) in enemyObjectsAndPositions)
            {
                var newEnemy = Instantiate(enemyScriptableObject.enemyPrefab, Vector3.zero, Quaternion.identity);
                var data = newEnemy.GetComponent<EnemyDataMono>();
                
                // HP
                data.MAXHitPoints = enemyScriptableObject.hp;
                data.HitPoints = enemyScriptableObject.hp;
            
                // Position
                data.Position = pos;
            
                // Abilities
                data.Abilities = new(enemyScriptableObject.abilities);
            
                // Movement
                data.MovementSpeed = enemyScriptableObject.movementPoints;
                data.MovementPoints = enemyScriptableObject.movementPoints;
            
                // Setup enemy sprite
                newEnemy.GetComponent<SpriteRenderer>().sprite = enemyScriptableObject.sprite;
                Enemies.Add(newEnemy);
            }
        }

        public void GoToCampaign()
        {
            SceneMovement.LoadCampaign();
        }


        public void GameOver()
        {
            SceneMovement.LoadMainMenu();
        }

        // Check status of enemy/player
        // Only called at the end of a turn to prevent unnecessary calls.
        public void CheckStatus()
        {
            if (playerDataMono.HitPoints <= 0)
            {
                // Player dead
                GameOver();
            }

            // If enemies exist, return
            if(Enemies.Any(o => o.activeSelf))
            {
                return;
            }
        
            // If it's the final level, load the win screen instead of rewards
            if (CampaignMapDataStore.CurrentSquare == CampaignMapDataStore.FinalEncounterPos)
            {
                SceneMovement.LoadWinScreen();
            }
        
            // Deactivate player and enemy objects
            GameObject.FindWithTag("Player").SetActive(false);
            foreach (var enemy in Enemies)
            {
                enemy.SetActive(false);
            }
            ActivateRewardScreen();
        }
    

        private void FindPlayer()
        {
            var player = GameObject.FindWithTag("Player");

            if (player.TryGetComponent(out CharacterDataMono data))
            {
                playerDataMono = data;
            }
        }

        private void ActivateRewardScreen()
        {
            rewardScreen.SetActive(true);
        }

        public void UpdatePlayerData()
        {
            // Update hitpoints and currency
            PlayerDataStore.CharacterData.HitPoints = playerDataMono.HitPoints;
            PlayerDataStore.CharacterData.Currency += currentEncounter.currencyReward;
        }

        public List<Vector3Int> GetEnemyPositions()
        {
            var positions = new List<Vector3Int>();
            foreach (var enemy in Enemies.Where(x => x.activeSelf))
            {
                if (enemy.TryGetComponent(out CharacterDataMono data))
                {
                    positions.Add(data.Position);
                }
            }
            return positions;
        }

        public GameObject GetEnemyAtPosition(Vector3Int pos)
        {
            var activeEnemies = Enemies.Where(x => x.activeSelf);

            // Try to find an enemy that satisfies the query.
            foreach (var activeEnemy in activeEnemies)
            {
                if (!activeEnemy.TryGetComponent(out CharacterDataMono data)) continue;
            
                if (data.Position == pos)
                {
                    return activeEnemy;
                }
            }

            return null;
        }
    }
}
