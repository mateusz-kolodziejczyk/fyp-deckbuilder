using System.Collections.Generic;
using Character;
using ScriptableObjects;

namespace Statics
{
    public static class PlayerDataStore
    {
        public static CharacterData CharacterData { get; set; }

        public static List<CardScriptableObject> Deck { get; set; } = new();
    }
}
