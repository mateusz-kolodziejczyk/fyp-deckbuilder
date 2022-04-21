using TMPro;
using UnityEngine;

namespace Character
{
    [RequireComponent(typeof(CharacterDataMono))]
    public class Resource : MonoBehaviour
    {
        private CharacterDataMono dataMono;

        [SerializeField]
        private TextMeshProUGUI resourceText;
        // Start is called before the first frame update
        void Start()
        {
            dataMono = GetComponent<CharacterDataMono>();
            UpdateText();
        }

        public void UpdateResources(int resource)
        {
            dataMono.ResourceAmount += resource;
        }

        public bool HasEnoughResources(int resourceCost)
        {
            return dataMono.ResourceAmount >= resourceCost;
        }
        
        public void UpdateText()
        {
            // If max resource is 0, it hasn't been initialised yet;
            var maxResource = dataMono.MAXResource == 0 ? dataMono.ResourceAmount : dataMono.MAXResource;

            var s = $"{dataMono.ResourceAmount}/{maxResource}";

            resourceText.text = s;
        }
    }
}