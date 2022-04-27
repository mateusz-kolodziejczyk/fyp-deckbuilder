using System.Collections.Generic;
using Character;
using Enums;
using Helper;
using Player;
using ScriptableObjects;
using UnityEngine;

namespace Enemy
{
    [RequireComponent(typeof(AbilityChooser))]
    [RequireComponent(typeof(PlayerHolder))]
    public class EnemyAttack : MonoBehaviour
    {
        private AbilityChooser abilityChooser;

        private List<Vector3Int> squaresToAttack = new ();

        public List<Vector3Int> SquaresToAttack
        {
            get => squaresToAttack;
            set => squaresToAttack = value;
        }

        public EnemyAbilityScriptableObject CurrentAbility { get; private set; }

        private PlayerHolder playerHolder;

        private CharacterDataMono dataMono;
        // Start is called before the first frame update
        private void Start()
        {
            abilityChooser = GetComponent<AbilityChooser>();
            playerHolder = GetComponent<PlayerHolder>();
            dataMono = GetComponent<CharacterDataMono>();
        }



        public void CalculateSquaresToAttack()
        {
            // Clear the list of squares to attack
            squaresToAttack = new();
            CurrentAbility = abilityChooser.GetNextAbility();
            squaresToAttack = GridHighlightHelper.CalculateHighlightedSquares(dataMono.Position, CurrentAbility.range, CurrentAbility.targetingPattern);
        }
        public void Attack()
        {
            if (!playerHolder.Player.TryGetComponent(out CharacterDataMono playerData)) return;
        

            if (!squaresToAttack.Contains(playerData.Position)) return;
        
            if (CurrentAbility.abilityType != AbilityType.Attack) return;
        
            if (!playerHolder.Player.TryGetComponent(out Health health)) return;
            
            health.UpdateHealth(-CurrentAbility.magnitude);
            health.UpdateHealthText();
        }
    }
}
