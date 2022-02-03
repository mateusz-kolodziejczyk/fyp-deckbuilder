using System;
using UnityEngine;

namespace Character
{
    public class CharacterData : MonoBehaviour
    {
        [SerializeField]
        private int hitPoints;

        [SerializeField] private int movementSpeed;

        [SerializeField] private Vector3Int position;

        [SerializeField] private int resourceAmount;
        public Vector3Int Position
        {
            get => position;
            set => position = value;
        }

        public int MovementSpeed
        {
            get => movementSpeed;
            set => movementSpeed = value;
        }

        public int HitPoints
        {
            get => hitPoints;
            set => hitPoints = value;
        }

        public int MAXResource
        {
            get;
            private set;
        }

        public int ResourceAmount
        {
            get => resourceAmount;
            set => resourceAmount = value;
        }
        

        public int MAXHitPoints { get; private set; }
        public int TemporaryHitPoints { get; set; } = 0;

        public string Tag { get; set; }

        private void Start()
        {
            Tag = gameObject.tag;
            MAXHitPoints = hitPoints;
            MAXResource = resourceAmount;
        }
        
        public CharacterData()
        {
            hitPoints = 100;
            Position = new Vector3Int(0,0, 0);
        }

        public CharacterData(int hitPoints)
        {
            HitPoints = hitPoints;
        }
        
    }
}
