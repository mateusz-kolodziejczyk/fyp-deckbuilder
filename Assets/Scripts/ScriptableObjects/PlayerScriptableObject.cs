using System.Collections.Generic;
using Enums;
using UnityEngine;

namespace ScriptableObjects
{
    [CreateAssetMenu(fileName = "Data", menuName = "Characters/Player", order = 1)]
    public class PlayerScriptableObject : CharacterScriptableObject
    {
        public List<CardScriptableObject> startDeck;
        public int startCurrency;
        public int startResource;
    }
}
