using System.Collections.Generic;
using ScriptableObjects;
using UnityEditor.Experimental.GraphView;
using UnityEngine;

namespace Character
{
    public class EnemyData : CharacterData
    {
        [SerializeField]
        private List<EnemyAbilityScriptableObject> abilities;

        public List<EnemyAbilityScriptableObject> Abilities
        {
            get => abilities;
            private set => abilities = value;
        }
    }
}
