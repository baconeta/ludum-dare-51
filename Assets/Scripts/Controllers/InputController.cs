using UnityEngine;
using UnityEngine.InputSystem;

namespace Controllers
{
    public class InputController : MonoBehaviour
    {
        public PlayerInput playerInput;
        public static bool isMobile = false;

        private void SetControlsToMobile()
        {
            playerInput.SwitchCurrentActionMap("PlayerMobile");
            isMobile = true;
            //Show Mobile UI
        }

        private void SetControlsToKeyboard()
        {
            playerInput.SwitchCurrentActionMap("Player");
            isMobile = false;
            //Hide mobile UI
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