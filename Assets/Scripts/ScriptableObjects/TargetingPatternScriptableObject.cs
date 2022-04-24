using System.Collections.Generic;
using UnityEngine;

namespace ScriptableObjects
{
    namespace ScriptableObjects
    {
        [CreateAssetMenu(fileName = "Data", menuName = "Targeting/Pattern", order = 1)]
        public class TargetingPatternScriptableObject : ScriptableObject
        {
            public string prefabName; 
            public List<Vector2Int> directions;
            public int gap;
            public bool repeat = true;
        }
    }
}