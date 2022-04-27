using Enums;
using UnityEngine.SceneManagement;

namespace SceneManagement
{
    public static class SceneMovement
    {
        public static void LoadMainMenu()
        {
            SceneManager.LoadScene(SceneMatcher.SceneTypeToIndex[SceneType.MainMenu]);
        }

        public static void LoadCombat()
        {
            SceneManager.LoadScene(SceneMatcher.SceneTypeToIndex[SceneType.Battle]);
        }

        public static void LoadCampaign()
        {
            SceneManager.LoadScene(SceneMatcher.SceneTypeToIndex[SceneType.CampaignMap]);
        }

        public static void LoadShop()
        {
            SceneManager.LoadScene(SceneMatcher.SceneTypeToIndex[SceneType.Shop]);
        }

        public static void LoadWinScreen()
        {
            SceneManager.LoadScene(SceneMatcher.SceneTypeToIndex[SceneType.Win]);
        }

        public static void LoadGameOver()
        {
            SceneManager.LoadScene(SceneMatcher.SceneTypeToIndex[SceneType.GameOver]);
        }
    }
}
