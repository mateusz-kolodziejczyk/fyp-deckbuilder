using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace UI
{
    public class HealthBar : MonoBehaviour
    {
        [SerializeField] private Image healthBarImage;
        [SerializeField] private TextMeshProUGUI healthNumber;
        private float startXScale;

        private bool setScale = false;
        // Start is called before the first frame update
        private void Start()
        {
            startXScale = transform.localScale.x;
            setScale = true;
        }
    

        public void UpdateHealthBar(int maxHealth, int currentHealth)
        {
            if (!setScale)
            {
                return;
            }

            var newFillAmount = ((float) currentHealth / (float) maxHealth);
            healthBarImage.fillAmount = newFillAmount;        
            // Text
            healthNumber.text = currentHealth.ToString();
        }
    }
}
