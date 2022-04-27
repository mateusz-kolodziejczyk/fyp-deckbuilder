using UnityEngine;

namespace Character
{
    [RequireComponent(typeof(CharacterDataMono))]
    public class EnemyHealth : Health
    {
        [SerializeField] private HealthBar healthBar;

        public override bool UpdateHealthText()
        {
            if (DataMono == null) return false;
            
            healthBar.UpdateHealthBar(DataMono.MAXHitPoints, DataMono.HitPoints);
            return true;
        }
    }
}
