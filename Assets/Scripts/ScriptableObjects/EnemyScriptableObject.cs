using System.Collections.Generic;
using UnityEngine;

namespace ScriptableObjects
{
    [CreateAssetMenu(fileName = "Data", menuName = "Characters/Enemy", order = 1)]
    public class EnemyScriptableObject : CharacterScriptableObject
    {
        public List<EnemyAbilityScriptableObject> abilities;
        public GameObject enemyPrefab;
    }
}
