using UnityEngine;

namespace SceneManagement
{
    public class SceneMovementMono : MonoBehaviour
    {
        public void LoadMainMenu()
        {
            SceneMovement.LoadMainMenu();

        }

        public void LoadCombat()
        {
            SceneMovement.LoadCombat();

        }

        public void LoadCampaign()
        {
            SceneMovement.LoadCampaign();
        }

        public void LoadShop()
        {
            SceneMovement.LoadShop();
        }
    }
}
