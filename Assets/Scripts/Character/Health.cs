using System;
using TMPro;
using UnityEngine;

namespace Character
{
    [RequireComponent(typeof(CharacterDataMono))]
    public class Health : MonoBehaviour
    {
        private CharacterDataMono dataMono;

        [SerializeField]
        private TextMeshProUGUI healthText;
        // Start is called before the first frame update
        void Start()
        {
            dataMono = GetComponent<CharacterDataMono>();
            if (healthText != null)
            {
                UpdateHealthText();
            }
        }

        public void UpdateHealth(int hp)
        {
            while (dataMono.TemporaryHitPoints > 0 && hp != 0)
            {
                // This adds -1 if hp is negative, 1 if positive
                var sign = hp > 0 ? 1 : -1;
                dataMono.TemporaryHitPoints += 1*sign;
                hp += sign;
            }

            if (hp == 0)
            {
                return;
            }
            
            dataMono.HitPoints += hp;
        }

        public void AddTemporaryHP(int hp)
        {
            dataMono.TemporaryHitPoints += hp;
        }
        public bool IsAlive()
        {
            return dataMono.HitPoints > 0;
        }

        public void UpdateHealthText()
        {
            // If max hitpoints are 0, they haven't been initialised yet.
            var maxHitPoints = dataMono.MAXHitPoints == 0 ? dataMono.HitPoints : dataMono.MAXHitPoints;
            // If the character is not alive, set the text to say "Dead"
            if (!IsAlive())
            {
                healthText.text = $"{tag} is Dead";
                return;
            }
            
            var s = $"{dataMono.HitPoints}/{maxHitPoints}";
            
            // IF there are any temporary hit points, add them at the end.
            if (dataMono.TemporaryHitPoints > 0)
            {
                s += $" + {dataMono.TemporaryHitPoints}";
            }

            healthText.text = s;
        }
    }
}
