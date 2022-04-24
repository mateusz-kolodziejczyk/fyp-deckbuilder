using System.Collections;
using System.Collections.Generic;
using Character;
using Enums;
using Helper;
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
        set => squaresToAttack = value;
    }

    private EnemyAbilityScriptableObject currentAbility;

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
        currentAbility = abilityChooser.GetNextAbility();
        var directions = HelperConstants.adjacentAddition;
        foreach (var direction in directions)
        {
            for (int i = 1; i <= currentAbility.range; i++)
            {
                squaresToAttack.Add(i*direction + dataMono.Position);
            }
        }
    }
    public void Attack()
    {
        if (!playerHolder.Player.TryGetComponent(out CharacterDataMono playerData)) return;
        

        if (!squaresToAttack.Contains(playerData.Position)) return;
        
        if (currentAbility.abilityType != CardType.Attack) return;
        
        if (!playerHolder.Player.TryGetComponent(out Health health)) return;
            
        health.UpdateHealth(-currentAbility.magnitude);
        health.UpdateHealthText();
    }
}
