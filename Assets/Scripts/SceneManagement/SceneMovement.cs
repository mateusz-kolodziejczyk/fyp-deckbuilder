using UnityEngine;
using UnityEngine.SceneManagement;

namespace SceneManagement
{
    public class SceneMovement : MonoBehaviour
    {
        public void loadMainMenu()
        {
            SceneManager.LoadScene(0);
        }

        public void loadCombat()
        {
            SceneManager.LoadScene(1);
        }
    }
}
