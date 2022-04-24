using System.Collections.Generic;
using ScriptableObjects;
using UnityEngine;

namespace Statics
{
    public static class CampaignMapDataStore
    {
        public static Vector2Int CurrentSquare { get; set; } = new (0, -1);
        public static Dictionary<Vector2Int, HashSet<Vector2Int>> Connections { get; set; } = new();

        public static Dictionary<Vector2Int, EncounterScriptableObject> EncounterScriptableObjects { get; set; } =
            new();

        public static EncounterScriptableObject BossEncounterScriptableObject { get; set; }
        public static void ResetData()
        {
            CurrentSquare = new (0, -1);
            Connections = new();
            EncounterScriptableObjects = new();
            BossEncounterScriptableObject = null;
        }
    }
}