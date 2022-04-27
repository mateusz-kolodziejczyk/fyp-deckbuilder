using UnityEngine;

namespace Options
{
    public class OptionsMenuManagement : MonoBehaviour
    {
        public bool IsActive { get; set; } = true;
        // Start is called before the first frame update
        void Start()
        {
            ToggleActive();
        }

        public void ToggleActive()
        {
            IsActive = !IsActive;
            
            foreach (Transform child in transform)
            {
                var childGameObject = child.gameObject;
                childGameObject.SetActive(!childGameObject.activeSelf);
            }
        }
    }
}
