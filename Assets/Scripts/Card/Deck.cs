using System.Collections.Generic;
using ScriptableObjects;
using UI;
using UnityEngine;
using UnityEngine.Serialization;

namespace Card
{
    public class Deck : MonoBehaviour
    {
        [SerializeField]
        private List<CardScriptableObject> cards;

        public List<CardScriptableObject> Cards
        {
            get => cards;
            set => cards = value;
        }
    
        [SerializeField]
        private GameObject uiDeck;

        private DeckDrawer _deckDrawer;


        // Start is called before the first frame update
        void Start()
        {
            _deckDrawer = uiDeck.GetComponent<DeckDrawer>();
        }

        // Update is called once per frame
        void Update()
        {
            _deckDrawer.UpdateCards(cards);
        }
    
    
    }
}
