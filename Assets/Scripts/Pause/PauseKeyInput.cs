using System;
using UnityEngine;

namespace Pause
{
    public class PauseKeyInput : MonoBehaviour
    {
        private PauseManagement pauseManagement;
        private void Start()
        {
            pauseManagement = GetComponent<PauseManagement>();
            TurnMenuOnOff();
        }

        // Update is called once per frame
        private void Update()
        {
            // If escape is pressed, activate/deactivate all child elements
            if (Input.GetKeyDown(KeyCode.Escape))
            {
                TurnMenuOnOff();
            }
        }

        private void TurnMenuOnOff()
        {
            pauseManagement.IsActive = !pauseManagement.IsActive;

            foreach (Transform child in transform)
            {
                var childGameObject = child.gameObject;
                childGameObject.SetActive(!childGameObject.activeSelf);
            }
        }
    }
}
