using System.Collections;
using System.Collections.Generic;
using Enums;
using ScriptableObjects;
using UnityEngine;

public class EncounterData : MonoBehaviour
{
    [SerializeField] private EncounterScriptableObject encounterScriptableObject;

    public EncounterScriptableObject EncounterScriptableObject
    {
        get => encounterScriptableObject;
        set
        {
            encounterScriptableObject = value;
            LoadImage();
        }
    }

    private void LoadImage()
    {
        GetComponent<SpriteRenderer>().sprite = encounterScriptableObject.image;
    }
}
