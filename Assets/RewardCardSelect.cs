using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class RewardCardSelect : MonoBehaviour, IPointerClickHandler
{

    private RewardCardManagement cardManagement;

    private int index;
    // Start is called before the first frame update
    void Start()
    {
        cardManagement = transform.parent.GetComponent<RewardCardManagement>();
        index = transform.GetSiblingIndex();
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        if (cardManagement != null)
        {
            cardManagement.SelectCard(index);
            Debug.Log(index);
        }
    }
}
