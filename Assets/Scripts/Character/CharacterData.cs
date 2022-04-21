using UnityEngine;
using UnityEngine.UIElements;

namespace Character
{
    public class CharacterData
    {
        public Vector3Int Position { get; set; }

        public int MovementSpeed { get; set; }

        public int HitPoints { get; set; }

        public int MovementPoints
        {
            get;
            set;
        }

        public int MAXResource
        {
            get;
            set;
        }

        public int ResourceAmount { get; set; }

        public int MAXHitPoints { get; set; }
        public int TemporaryHitPoints { get; set; }

        public string Tag { get; set; }

        public int Currency { get; set; }

        public CharacterData()
        {
            
        }

        public CharacterData(Vector3Int position, int movementSpeed, int maxResource, int maxHitPoints, int currency)
        {
            Position = position;
            
            MovementSpeed = movementSpeed;
            MovementPoints = movementSpeed;
            
            MAXResource = maxResource;
            ResourceAmount = maxResource;
            
            MAXHitPoints = maxHitPoints;
            HitPoints = maxHitPoints;
            
            Currency = currency;
        }
    }
}