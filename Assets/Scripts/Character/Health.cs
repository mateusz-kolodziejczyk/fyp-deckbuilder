using TMPro;
using UnityEngine;

namespace Character
{
    [RequireComponent(typeof(CharacterData))]
    public class Health : MonoBehaviour
    {
        private CharacterData _data;

        [SerializeField]
        private TextMeshProUGUI healthText;
        // Start is called before the first frame update
        void Start()
        {
            _data = GetComponent<CharacterData>();
            UpdateHealthText();
        }

        public void UpdateHealth(int hp)
        {
            _data.HitPoints += hp;
        }

        public void AddTemporaryHP(int hp)
        {
            _data.TemporaryHitPoints += hp;
        }
        public bool IsAlive()
        {
            return _data.HitPoints > 0;
        }

        public void UpdateHealthText()
        {
            // If max hitpoints are 0, they haven't been initialised yet.
            var maxHitPoints = _data.MAXHitPoints == 0 ? _data.HitPoints : _data.MAXHitPoints;
            // If the character is not alive, set the text to say "Dead"
            if (!IsAlive())
            {
                healthText.text = $"{tag} is Dead";
                return;
            }
            
            var s = $"{tag} HP {_data.HitPoints}/{maxHitPoints}";
            
            // IF there are any temporary hit points, add them at the end.
            if (_data.TemporaryHitPoints > 0)
            {
                s += $" + {_data.TemporaryHitPoints}";
            }

            healthText.text = s;
        }
    }
}
