using Enums;
using UnityEngine;

namespace ScriptableObjects
{
    [CreateAssetMenu(fileName = "Data", menuName = "Encounter/Shop", order = 1)]
    public class ShopScriptableObject : EncounterScriptableObject
    {
        public EncounterType type = EncounterType.Shop;
        public float itemRarity;
    }
}