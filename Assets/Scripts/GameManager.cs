using System.Collections;
using System.Collections.Generic;
using Character;
using SceneManagement;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    private List<EnemyData> enemyData = new();

    private CharacterData playerData;
    // Start is called before the first frame update
    void Start()
    {
        FindEnemies();
        FindPlayer();
    }

    // Update is called once per frame
    void Update()
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
        if (playerData.HitPoints <= 0)
        {
            // Player dead
            GameOver();
        }

        // Start off assuming all enemies are dead, if one of them is alive make it false
        var allEnemiesDead = true;

        foreach (var data in enemyData)
        {
            if (data.HitPoints > 0)
            {
                allEnemiesDead = false;
            }
        }

        if (allEnemiesDead)
        {
            GoToCampaign();
        }
    }

    private void FindEnemies()
    {
        var enemies = GameObject.FindGameObjectsWithTag("Enemy");

        foreach (var enemy in enemies)
        {
            if (enemy.TryGetComponent(out EnemyData data))
            {
                enemyData.Add(data);
            }
        }
    }

    private void FindPlayer()
    {
        var player = GameObject.FindWithTag("Player");

        if (player.TryGetComponent(out CharacterData data))
        {
            playerData = data;
        }
    }
}
