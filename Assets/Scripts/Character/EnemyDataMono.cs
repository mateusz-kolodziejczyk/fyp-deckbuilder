using System.Collections.Generic;
using ScriptableObjects;

namespace Character
{
    public class EnemyDataMono : CharacterDataMono
    {
        public List<EnemyAbilityScriptableObject> Abilities { get; set; } = new();
    }
}
