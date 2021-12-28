using Enums;
using UnityEngine;
using UnityEngine.Serialization;

namespace ScriptableObjects
{
    public class CardScriptableObject : ScriptableObject
    {
        public string prefabName;
        
        public int resourceCost;
        public int goldValue;
        
        public string description;
    }
}
