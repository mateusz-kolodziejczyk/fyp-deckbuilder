using System;
using UnityEngine;

namespace Pause
{
    public class PauseManagement : MonoBehaviour
    {
        public bool IsActive { get; private set; } = true;

        private void Start()
        {
            TurnMenuOnOff();
        }

        public void TurnMenuOnOff()
        {
            IsActive = !IsActive;

            foreach (Transform child in transform)
            {
                var childGameObject = child.gameObject;
                childGameObject.SetActive(!childGameObject.activeSelf);
            }
        }
    }
}
