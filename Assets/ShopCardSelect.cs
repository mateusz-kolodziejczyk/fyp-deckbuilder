using UnityEngine;
using UnityEngine.EventSystems;

public class ShopCardSelect : MonoBehaviour, IPointerClickHandler, IPointerEnterHandler, IPointerExitHandler
{

    private ManageShop manageShop;


    private AnimationScaler animationScaler;

    private Coroutine animationCoroutine;

    private ShopCardItem shopCardItem;
    // Start is called before the first frame update
    void Start()
    {
        manageShop = GameObject.FindWithTag("ShopManager").GetComponent<ManageShop>();
        animationScaler = GetComponent<AnimationScaler>();
        shopCardItem = transform.parent.GetComponent<ShopCardItem>();
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        if (manageShop == null) return;
        if (manageShop.BuyCard(shopCardItem.Card))
        {
            transform.parent.gameObject.SetActive(false);
        }
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        if (animationCoroutine != null)
        {
            StopCoroutine(animationCoroutine);

        }
        animationCoroutine = StartCoroutine(animationScaler.ResetBoxSize());
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        if (animationCoroutine != null)
        {
            StopCoroutine(animationCoroutine);

        }
        animationCoroutine = StartCoroutine(animationScaler.IncreaseBoxSize());
    }
}
