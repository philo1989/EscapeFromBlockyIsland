using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class EventTriggerTest : MonoBehaviour
{
    InputAction.CallbackContext context;
    // Update is called once per frame
    void Update()
    {

        
        if (context.started)
        {
            Debug.Log("SpawjNmaan");
            EventManager.TriggerEvent("test");
        }
    }
}
