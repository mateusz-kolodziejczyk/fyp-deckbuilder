using ScriptableObjects;
using TMPro;
using UnityEngine;

public class ShopCardItem : MonoBehaviour
{
    public CardScriptableObject Card { get; set; }
    public int Index { get; set; } = -1;

    [SerializeField] private TextMeshProUGUI nameText, description, resourceCost, moneyCost;
    
    public void SetFields()
    {
        if (Card == null)
        {
            return;
        }
        
        nameText.text = Card.prefabName;
        description.text = Card.description;
        resourceCost.text = Card.resourceCost.ToString();
        moneyCost.text = $"${Card.goldValue}";
    }
}
