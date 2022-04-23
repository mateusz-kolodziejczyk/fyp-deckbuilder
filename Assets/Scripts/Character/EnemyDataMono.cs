using System.Collections.Generic;
using ScriptableObjects;
using UnityEngine;

namespace Character
{
    public class EnemyDataMono : CharacterDataMono
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
