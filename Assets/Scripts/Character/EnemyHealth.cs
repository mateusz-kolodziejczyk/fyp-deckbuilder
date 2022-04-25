using UnityEngine;

namespace Character
{
    [RequireComponent(typeof(CharacterDataMono))]
    public class EnemyHealth : Health
    {
        [SerializeField] private HealthBar healthBar;

        public override void UpdateHealthText()
        {
            healthBar.UpdateHealthBar(DataMono.MAXHitPoints, DataMono.HitPoints);
        }
    }
}
