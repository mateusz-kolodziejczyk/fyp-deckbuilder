using System.Collections.Generic;
using ScriptableObjects;
using UnityEngine;

namespace RewardScreen
{
    public class PossibleRewardCards : MonoBehaviour
    {
        [SerializeField] private List<CardScriptableObject> cards = new();
    
        public List<CardScriptableObject> Cards 
        {
            get => cards;
            set => cards = value;
        }
    }
}
