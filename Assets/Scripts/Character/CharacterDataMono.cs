using UnityEngine;

namespace Character
{
    public class CharacterDataMono : MonoBehaviour
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

        public int ResourceAmount
        {
            get => resourceAmount;
            set => resourceAmount = value;
        }
        

        public int MAXHitPoints { get; set; }
        public int TemporaryHitPoints { get; set; } = 0;

        public string Tag { get; set; }

        public int Currency { get; set; } = 10;

        public virtual void Start()
        {
            Tag = gameObject.tag;
            MAXHitPoints = hitPoints;
            MAXResource = resourceAmount;
            MovementPoints = movementSpeed;
        }
        

        public void UseMovementPoints(int pointsToUse)
        {
            MovementPoints -= pointsToUse;
            if (MovementPoints < 0)
            {
                MovementPoints = 0;
            }
        }

        public void ResetMovementPoints()
        {
            MovementPoints = MovementSpeed;
        }
        
    }
}
