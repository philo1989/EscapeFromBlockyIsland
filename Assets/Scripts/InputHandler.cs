using StarterAssets;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Controls;

public class InputHandler : MonoBehaviour
{
    public StarterAssetsInputs _input;
    //InputAction.CallbackContext context;
    
    
    // Update is called once per frame
    void Update()
    {


        // An action that triggers every time any button on the gamepad is
        // pressed or released.
        var action = new InputAction(
            type: InputActionType.PassThrough,
            binding: "<Keyboard>/<Button>");

        action.performed +=
            ctx =>
            {
                var button = (ButtonControl)ctx.control;
                if (button.wasPressedThisFrame) { 
                    if (button.Equals("escape"))
                    Debug.Log($"Button {ctx.control} was pressed");
                }
                else if (button.wasReleasedThisFrame)
                    Debug.Log($"Button {ctx.control} was released");
                // NOTE: We may get calls here in which neither the if nor the else
                //       clause are true here. A button like the gamepad left and right
                //       triggers, for example, do not just have a binary on/off state
                //       but rather a [0..1] value range.
            };

        action.Enable();
    }
}
