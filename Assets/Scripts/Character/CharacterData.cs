using UnityEngine;

namespace Character
{
    public class CharacterData : MonoBehaviour
    {
        [SerializeField]
        private int hitPoints;

        public int HitPoints
        {
            get => hitPoints;
            set => hitPoints = value;
        }
        public (int x, int y) Position { get; set; }

        public CharacterData()
        {
            hitPoints = 100;
            Position = (0, 0);
        }

        public CharacterData(int hitPoints)
        {
            HitPoints = hitPoints;
        }
    }
}
