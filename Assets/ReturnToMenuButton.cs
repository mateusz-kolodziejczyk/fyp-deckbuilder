using SceneManagement;
using Statics;
using UnityEngine;

public class ReturnToMenuButton : MonoBehaviour
{
    public void ReturnToMainMenu()
    {
        SceneMovement.LoadMainMenu();
        CampaignMapDataStore.ResetData();
        PlayerDataStore.ResetData();
    }
}
