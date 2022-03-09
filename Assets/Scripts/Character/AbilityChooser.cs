using System.Collections;
using System.Collections.Generic;
using Character;
using ScriptableObjects;
using UnityEngine;

public class AbilityChooser : MonoBehaviour
{
    private EnemyData data;

    // Start is called before the first frame update
    void Start()
    {
        data = GetComponent<EnemyData>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public EnemyAbilityScriptableObject GetNextAbility()
    {
        // TODO: Do proper sequence of abilities
        return data.Abilities[0];
    }
}
