using System;
using System.Collections;
using System.Collections.Generic;
using SceneManagement;
using ScriptableObjects;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;

[RequireComponent(typeof(EncounterAnimationHandler))]
public class EncounterInteraction : MonoBehaviour
{
    private EncounterAnimationHandler animationHandler;

    private Coroutine animationCoroutine;

    public bool Active { get; set; } = true;
    public Vector2 Position { get; set; } = Vector2.negativeInfinity;
    private bool isFaded = false;
    
    private void Start()
    {
        animationHandler = GetComponent<EncounterAnimationHandler>();
    }

    private void Update()
    {
        if (!isFaded && !Active)
        {
            FadeOut();
        }
    }

    private void FadeOut()
    {
        var spriteRenderer = GetComponent<SpriteRenderer>();

        var color = spriteRenderer.color;
        color = new Color(color.r, color.g, color.b, 0.5f);
        spriteRenderer.color = color;
    }
    public void OnMouseDown()
    {
        if (!Active)
        {
            return;
        }
        Debug.Log("Clicked");
        if (TryGetComponent(out EncounterData data))
        {
            if (data.EncounterScriptableObject is BattleScriptableObject)
            {
                SceneMovement.LoadCombat();
            }
            else if (data.EncounterScriptableObject is ShopScriptableObject)
            {
                SceneMovement.LoadShop();
            }
        }
    }

    public void OnMouseEnter()
    {
        
        if (animationCoroutine != null)
        {
            StopCoroutine(animationCoroutine);

        }
        if (!Active)
        {
            return;
        }
        animationCoroutine = StartCoroutine(animationHandler.IncreaseBoxSize());
    }

    public void OnMouseExit()
    { 
        
        if (animationCoroutine != null)
        {
            StopCoroutine(animationCoroutine);

        }
        if (!Active)
        {
            return;
        }
        StartCoroutine(animationHandler.ResetBoxSize());
    }
}
