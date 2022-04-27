using Enemy;
using ScriptableObjects;
using UnityEngine;

namespace Character
{
    public class AbilityChooser : MonoBehaviour
    {
        private EnemyDataMono dataMono;

        // Start is called before the first frame update
        void Start()
        {
            dataMono = GetComponent<EnemyDataMono>();
        }

        // Update is called once per frame
        void Update()
        {
        
        }

        public EnemyAbilityScriptableObject GetNextAbility()
        {
            // TODO: Do proper sequence of abilities
            return dataMono.Abilities[0];
        }
    }
}
