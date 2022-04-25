using UnityEngine.SceneManagement;

namespace SceneManagement
{
    public static class SceneMovement
    {
        public static void LoadMainMenu()
        {
            SceneManager.LoadScene(0);
        }

        public static void LoadCombat()
        {
            SceneManager.LoadScene(1);
        }

        public static void LoadCampaign()
        {
            SceneManager.LoadScene(2);
        }

        public static void LoadShop()
        {
            SceneManager.LoadScene(3);
        }

        public static void LoadWinScreen()
        {
            SceneManager.LoadScene(4);
        }

        public static void LoadGameOver()
        {
            SceneManager.LoadScene(5);
        }
    }
}
