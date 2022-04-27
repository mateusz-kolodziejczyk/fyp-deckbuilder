using UnityEngine;

namespace Options
{
    public class OptionsMenuManagement : MonoBehaviour
    {
        private GameObject optionsMenu;
        // Start is called before the first frame update
        void Start()
        {
            optionsMenu = GameObject.FindWithTag("OptionsMenu");
            optionsMenu.SetActive(false);
        }

        public void ToggleActive()
        {
            optionsMenu.SetActive(!optionsMenu.activeSelf);
        }

    }
}
