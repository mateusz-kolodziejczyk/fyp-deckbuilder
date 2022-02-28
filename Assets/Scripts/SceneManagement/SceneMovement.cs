using UnityEngine;
using UnityEngine.SceneManagement;

namespace SceneManagement
{
    public static class SceneMovement
    {
        public static void loadMainMenu()
        {
            SceneManager.LoadScene(0);
        }

        public static void loadCombat()
        {
            SceneManager.LoadScene(1);
        }
    }
}
