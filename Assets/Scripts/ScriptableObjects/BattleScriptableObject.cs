using System.Collections.Generic;
using Enums;
using UnityEngine;

namespace ScriptableObjects
{
    [CreateAssetMenu(fileName = "Data", menuName = "Encounter/Battle", order = 1)]
    public class BattleScriptableObject : EncounterScriptableObject
    {
        public List<GameObject> enemies;
        public List<Vector3Int> enemyPositions;
        public Vector3Int playerStartPosition;
    }
}