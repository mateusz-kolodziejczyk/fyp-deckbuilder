using Enums;
using ScriptableObjects.ScriptableObjects;
using UnityEngine;

namespace ScriptableObjects
{
    [CreateAssetMenu(fileName = "Data", menuName = "Cards/SimpleCard", order = 1)]
    public class SimpleCardScriptableObject : CardScriptableObject
    {
        public AbilityType type;
        // magnitude is how much the card does. It is the number of damage it does as well as how much defense it adds.
        public int magnitude;
        public TargetingPatternScriptableObject targetingPattern;
    }
}