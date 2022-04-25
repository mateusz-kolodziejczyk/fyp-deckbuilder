using UnityEngine;

namespace ScriptableObjects
{
    public class CharacterScriptableObject : ScriptableObject
    {
        public string prefabName;

        public int movementPoints;
        public int hp;
        public Sprite sprite;
    }
}
