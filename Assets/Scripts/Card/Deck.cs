using System;
using System.Collections.Generic;
using ScriptableObjects;
using Statics;
using UI;
using UnityEngine;
using UnityEngine.Serialization;
using Random = UnityEngine.Random;

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


        public CardScriptableObject DrawCard()
        {
            if (cards.Count <= 0)
            {
                return null;
            }
            var c = cards[^1];
            cards.RemoveAt(cards.Count-1);
            return c;
        }

        private void Start()
        {
            Shuffle();
            var d = PlayerDataStore.Deck;
            // If it doesn't already exist, return;
            if (d == null)
            {
                PlayerDataStore.Deck = cards;
                return;
            }
            cards = d;
        }

        // Code for shuffling from https://stackoverflow.com/questions/273313/randomize-a-listt
        public void Shuffle()
        {
            var n = cards.Count;  
            while (n > 1) {  
                n--;  
                var k = Random.Range(0, n + 1);  
                (cards[k], cards[n]) = (cards[n], cards[k]);
            }  
            
        }
    }
}
