using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class InputFieldVolumeValidation : MonoBehaviour
{
    // Code from https://docs.unity3d.com/2018.3/Documentation/ScriptReference/UI.InputField-onValidateInput.html
    private TMP_InputField volumeInputField;
    // Start is called before the first frame update
    void Start()
    {
        volumeInputField = GetComponent<TMP_InputField>();
        volumeInputField.onValueChanged.AddListener(ValidateNumber);
    }

    private void ValidateNumber(string newValue)
    {
        // Convert string to int
        if (!int.TryParse(newValue, out var intVal)) return;

        if (intVal > 100)
        {
            volumeInputField.text = "100";
        }
        else if (intVal < 0)
        {
            volumeInputField.text = "0";
        }
    }
}
