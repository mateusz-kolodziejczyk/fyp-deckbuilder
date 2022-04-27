using System.Collections.Generic;
using Character;
using ScriptableObjects;

namespace Enemy
{
    public class EnemyDataMono : CharacterDataMono
    {
        public List<EnemyAbilityScriptableObject> Abilities { get; set; } = new();
    }
}
