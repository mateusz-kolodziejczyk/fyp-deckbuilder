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

    public Vector2Int Position { get; set; } = Vector2Int.zero;

    private void LoadImage()
    {
        GetComponent<SpriteRenderer>().sprite = encounterScriptableObject.image;
    }
}
