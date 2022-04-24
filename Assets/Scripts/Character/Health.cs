using System;
using TMPro;
using UnityEngine;

namespace Character
{
    [RequireComponent(typeof(CharacterDataMono))]
    public class Health : MonoBehaviour
    {
        private protected CharacterDataMono DataMono;

        [SerializeField]
        private TextMeshProUGUI healthText;
        // Start is called before the first frame update
        void Start()
        {
            DataMono = GetComponent<CharacterDataMono>();
            UpdateHealthText();
        }

        public void UpdateHealth(int hp)
        {
            while (DataMono.TemporaryHitPoints > 0 && hp != 0)
            {
                // This adds -1 if hp is negative, 1 if positive
                var sign = hp > 0 ? 1 : -1;
                DataMono.TemporaryHitPoints += 1*sign;
                hp += -1*sign;
            }

            if (hp == 0)
            {
                return;
            }
            
            DataMono.HitPoints += hp;
        }

        public void AddTemporaryHP(int hp)
        {
            DataMono.TemporaryHitPoints += hp;
        }
        public bool IsAlive()
        {
            return DataMono.HitPoints > 0;
        }

        public virtual void UpdateHealthText()
        {
            // If max hitpoints are 0, they haven't been initialised yet.
            var maxHitPoints = DataMono.MAXHitPoints == 0 ? DataMono.HitPoints : DataMono.MAXHitPoints;
            // If the character is not alive, set the text to say "Dead"
            if (!IsAlive())
            {
                healthText.text = $"{tag} is Dead";
                return;
            }
            
            var s = $"{DataMono.HitPoints}/{maxHitPoints}";
            
            // IF there are any temporary hit points, add them at the end.
            if (DataMono.TemporaryHitPoints > 0)
            {
                s += $" + {DataMono.TemporaryHitPoints}";
            }

            healthText.text = s;
        }
    }
}
