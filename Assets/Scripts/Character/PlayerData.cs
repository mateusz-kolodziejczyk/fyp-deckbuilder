using Statics;

namespace Character
{
    public class PlayerData : CharacterData
    {
        public override void Start()
        {
            base.Start();
            var pd = PlayerDataStore.CharacterData;
            // If it doesn't already exist, return;
            if (pd == null)
            {
                PlayerDataStore.CharacterData = this;
                return;
            }
            
            // Load in data
            // HP
            HitPoints = pd.HitPoints;
            MAXHitPoints = pd.MAXHitPoints;
            
            // Resource
            MAXResource = pd.MAXResource;
            ResourceAmount = MAXResource;
            
            // Movement Points
            MovementSpeed = pd.MovementSpeed;
            MovementPoints = pd.MovementSpeed;
        }
    }
}