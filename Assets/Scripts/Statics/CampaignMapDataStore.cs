using System.Collections.Generic;
using ScriptableObjects;
using UnityEngine;

namespace Statics
{
    public static class CampaignMapDataStore
    {
        public static Vector2Int CurrentSquare { get; set; } = new Vector2Int(0, -1);
        public static Dictionary<Vector2Int, HashSet<Vector2Int>> Connections { get; set; } = new();

        public static Dictionary<Vector2Int, EncounterScriptableObject> EncounterScriptableObjects { get; set; } =
            new();
    }
}