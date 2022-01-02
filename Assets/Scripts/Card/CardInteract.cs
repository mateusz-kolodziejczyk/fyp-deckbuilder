using System;
using UI;
using UnityEngine;
using UnityEngine.EventSystems;

namespace Card
{
    public class CardInteract : MonoBehaviour, IPointerClickHandler
    {
        [SerializeField]
        private DeckDrawer deckDrawer;

        private int _index;

        private void Start()
        {
            deckDrawer = transform.parent.gameObject.GetComponent<DeckDrawer>();
            _index = transform.GetSiblingIndex();
        }

        public void OnPointerClick(PointerEventData eventData)
        {
            if (deckDrawer != null)
            {
                deckDrawer.CardPlayer.PlayCard(_index);
            }
        }
    }
}
