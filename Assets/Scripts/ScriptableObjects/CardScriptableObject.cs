using UnityEngine;

namespace ScriptableObjects
{
    public class CardScriptableObject : ScriptableObject
    {
        public string prefabName;
        
        public int resourceCost;
        public int goldValue;

        public int range;
        
        public string description;
    }
}
