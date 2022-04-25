using TMPro;
using UnityEngine;

public class HealthBar : MonoBehaviour
{
    [SerializeField] private Transform healthTransform;
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
        
        var newXScale = ((float)currentHealth/maxHealth) * startXScale;
        var t = healthTransform;
        var localScale = t.localScale;
        localScale = new (newXScale, localScale.y, localScale.z);
        t.localScale = localScale;
        
        // Text
        healthNumber.text = currentHealth.ToString();
    }
}
