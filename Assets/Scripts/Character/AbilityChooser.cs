using Enemy;
using ScriptableObjects;
using UnityEngine;

namespace Character
{
    public class AbilityChooser : MonoBehaviour
    {
        private EnemyDataMono dataMono;
        private int currentAbilityIndex = 0;

        // Start is called before the first frame update
        void Start()
        {
            dataMono = GetComponent<EnemyDataMono>();
        }
        public EnemyAbilityScriptableObject GetNextAbility()
        {
            var newAbility =  dataMono.Abilities[currentAbilityIndex];
            
            currentAbilityIndex += 1;
            if (currentAbilityIndex >= dataMono.Abilities.Count)
            {
                currentAbilityIndex = 0;
            }
            
            return newAbility;
        }
    }
}
