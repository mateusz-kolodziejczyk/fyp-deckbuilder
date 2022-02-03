using TMPro;
using UnityEngine;

namespace Character
{
    [RequireComponent(typeof(CharacterData))]
    public class Resource : MonoBehaviour
    {
        private CharacterData data;

        [SerializeField]
        private TextMeshProUGUI resourceText;
        // Start is called before the first frame update
        void Start()
        {
            data = GetComponent<CharacterData>();
            UpdateText();
        }

        public void UpdateResources(int resource)
        {
            data.ResourceAmount += resource;
        }

        public bool HasEnoughResources(int resourceCost)
        {
            return data.ResourceAmount >= resourceCost;
        }
        
        public void UpdateText()
        {
            // If max resource is 0, it hasn't been initialised yet;
            var maxResource = data.MAXResource == 0 ? data.ResourceAmount : data.MAXResource;

            var s = $"{data.ResourceAmount}/{maxResource}";

            resourceText.text = s;
        }
    }
}