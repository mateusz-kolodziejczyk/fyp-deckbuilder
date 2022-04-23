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
        public List<CardScriptableObject> Cards { get; set; } = new();
        public List<CardScriptableObject> DiscardPile { get; set; } = new();

        public CardScriptableObject DrawCard()
        {
            if (Cards.Count <= 0)
            {
                return null;
            }
            var c = Cards[^1];
            Cards.RemoveAt(Cards.Count-1);
            return c;
        }

        public void ReshuffleDiscards()
        {
            if (DiscardPile.Count <= 0)
            {
                return;
            }
            Cards = DiscardPile;
            DiscardPile = new List<CardScriptableObject>();
            Shuffle();
        }
        
        // Code for shuffling from https://stackoverflow.com/questions/273313/randomize-a-listt
        public void Shuffle()
        {
            var n = Cards.Count;  
            while (n > 1) {  
                n--;  
                var k = Random.Range(0, n + 1);  
                (Cards[k], Cards[n]) = (Cards[n], Cards[k]);
            }  
            
        }
    }
}
