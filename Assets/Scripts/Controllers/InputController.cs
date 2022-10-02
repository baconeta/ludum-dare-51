using System;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Controllers
{
    public class InputController : MonoBehaviour
    {
        public PlayerInput playerInput;
        public static bool isMobile = false;
        public GameUI gameUI;

        private void Start()
        {
            if (!gameUI) gameUI = FindObjectOfType<GameUI>();
        }

        private void SetControlsToMobile()
        {
            playerInput.SwitchCurrentActionMap("PlayerMobile");
            isMobile = true;

            //Show Mobile UI
            gameUI.ShowMobileUI();
        }

        private void SetControlsToKeyboard()
        {
            playerInput.SwitchCurrentActionMap("Player");
            isMobile = false;

            //Hide mobile UI
            gameUI.HideMobileUI();
        }

        public void UseMobileControls(bool toggle)
        {
            if (toggle) SetControlsToMobile();
            else SetControlsToKeyboard();

            Debug.Log(playerInput.currentActionMap);
        }

        public bool GetIsMobile()
        {
            return isMobile;
        }
    }
}