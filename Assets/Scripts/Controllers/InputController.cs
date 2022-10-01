using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class InputController : MonoBehaviour
{
    public PlayerInput playerInput;
    public bool isMobile;

    
    void SetControlsToMobile()
    {
        playerInput.SwitchCurrentActionMap("PlayerMobile");
        isMobile = true;
        //Show Mobile UI

    }

    void SetControlsToKeyboard()
    {
        playerInput.SwitchCurrentActionMap("Player");
        isMobile = false;
        //Hide mobile UI
    }

    public void UseMobileControls(bool toggle)
    {
        if(toggle) SetControlsToMobile();
        else SetControlsToKeyboard();
        
        Debug.Log(playerInput.currentActionMap);
    }

    public bool GetIsMobile()
    {
        return isMobile;
    }
}
