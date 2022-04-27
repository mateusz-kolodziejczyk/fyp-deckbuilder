using Enums;
using ScriptableObjects.ScriptableObjects;
using UnityEngine;

namespace ScriptableObjects
{
    [CreateAssetMenu(fileName = "Data", menuName = "EnemyAbilities/Ability", order = 1)]

    public class EnemyAbilityScriptableObject : ScriptableObject
    {
        public string prefabName;

        public int range;
        public int magnitude;
        public AbilityType abilityType;
        public TargetingPatternScriptableObject targetingPattern;
    }
}
