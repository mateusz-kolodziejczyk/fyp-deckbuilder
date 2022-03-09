using System.Collections;
using System.Collections.Generic;
using Character;
using Enums;
using ScriptableObjects;
using Unity.VisualScripting;
using UnityEngine;

[RequireComponent(typeof(AbilityChooser))]
[RequireComponent(typeof(PlayerHolder))]
public class EnemyAttack : MonoBehaviour
{
    private AbilityChooser abilityChooser;

    private List<Vector3Int> squaresToAttack = new ();

    public List<Vector3Int> SquaresToAttack
    {
        get => squaresToAttack;
        private set => squaresToAttack = value;
    }

    private EnemyAbilityScriptableObject currentAbility;

    private PlayerHolder playerHolder;

    private CharacterData data;
    // Start is called before the first frame update
    private void Start()
    {
        abilityChooser = GetComponent<AbilityChooser>();
        playerHolder = GetComponent<PlayerHolder>();
        data = GetComponent<CharacterData>();
    }



    public void CalculateSquaresToAttack()
    {
        currentAbility = abilityChooser.GetNextAbility();
        var directions = new List<Vector3Int> {Vector3Int.up, Vector3Int.down, Vector3Int.left, Vector3Int.right};
        foreach (var direction in directions)
        {
            for (int i = 1; i <= currentAbility.range; i++)
            {
                squaresToAttack.Add(i*direction + data.Position);
            }
        }
    }
    public void Attack()
    {
        if (playerHolder.Player.TryGetComponent(out CharacterData playerData))
        {
            if (squaresToAttack.Contains(playerData.Position))
            {
                if (currentAbility.abilityType == CardType.Attack)
                {
                    playerData.HitPoints -= currentAbility.magnitude;
                }
            }
        }
    }
}
