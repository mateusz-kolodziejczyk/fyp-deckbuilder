using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Card;
using Character;
using Movement;
using SceneManagement;
using ScriptableObjects;
using Statics;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    private List<EnemyDataMono> enemyData = new();
    private GameObject rewardScreen;
    private CharacterDataMono playerDataMono;
    // Start is called before the first frame update
    void Start()
    {
        FindEnemies();
        FindPlayer();
        rewardScreen = GameObject.FindWithTag("RewardScreen");
        rewardScreen.SetActive(false);
        SetupPlayer();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void SetupPlayer()
    {
        var player = GameObject.FindWithTag("Player");

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
        }
        
        // Move the player to the correct location
        if (player.TryGetComponent(out PlayerMovement playerMovement))
        {
            playerMovement.UpdatePositionToDataPosition();
        }

    }

    private void SetupEnemies()
    {
        
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
        if(enemyData.Any(data => data.HitPoints > 0))
        {
            return;
        }

        
        // Deactivate player and enemy objects
        GameObject.FindWithTag("Player").SetActive(false);
        var enemies = GameObject.FindGameObjectsWithTag("Enemy");
        foreach (var enemy in enemies)
        {
            enemy.SetActive(false);
        }
        ActivateRewardScreen();
    }

    private void FindEnemies()
    {
        var enemies = GameObject.FindGameObjectsWithTag("Enemy");

        foreach (var enemy in enemies)
        {
            if (enemy.TryGetComponent(out EnemyDataMono data))
            {
                enemyData.Add(data);
            }
        }
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
        // Right now only update hitpoints, everything else is only changed after fight
        PlayerDataStore.CharacterData.HitPoints = playerDataMono.HitPoints;
        
    }
}
