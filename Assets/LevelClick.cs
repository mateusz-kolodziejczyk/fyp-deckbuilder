using System;
using System.Collections;
using System.Collections.Generic;
using SceneManagement;
using UnityEngine;
using UnityEngine.EventSystems;

[RequireComponent(typeof(EncounterAnimationHandler))]
public class LevelClick : MonoBehaviour
{
    private EncounterAnimationHandler animationHandler;

    private Coroutine animation;
    private void Start()
    {
        animationHandler = GetComponent<EncounterAnimationHandler>();
    }

    public void OnMouseDown()
    {
        Debug.Log("Clicked");
        SceneMovement.LoadCombat();
    }

    public void OnMouseEnter()
    {
        if (animation != null)
        {
            StopCoroutine(animation);

        }
        animation = StartCoroutine(animationHandler.IncreaseBoxSize());
    }

    public void OnMouseExit()
    { 
        if (animation != null)
        {
            StopCoroutine(animation);

        }
        StartCoroutine(animationHandler.ResetBoxSize());
    }
}
