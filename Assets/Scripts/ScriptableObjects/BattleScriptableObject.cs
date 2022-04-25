using System.Collections.Generic;
using UnityEngine;

namespace ScriptableObjects
{
    [CreateAssetMenu(fileName = "Data", menuName = "Encounter/Battle", order = 1)]
    public class BattleScriptableObject : EncounterScriptableObject
    {
        public List<EnemyScriptableObject> enemies;
        public List<Vector3Int> enemyPositions;
        public Vector3Int playerStartPosition;
        public int currencyReward;
    }
}