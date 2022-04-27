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
        }

        // Update is called once per frame
        private void Update()
        {
            // If escape is pressed, activate/deactivate all child elements
            if (Input.GetKeyDown(KeyCode.Escape))
            {
                pauseManagement.TurnMenuOnOff();
            }
        }
    }

}
