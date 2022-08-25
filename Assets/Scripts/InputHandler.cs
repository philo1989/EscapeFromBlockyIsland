using StarterAssets;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Controls;

public class InputHandler : MonoBehaviour
{
    public Canvas menu;
    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown("q"))
        {
            EventManager.TriggerEvent("test");
        }
        if (Input.GetKeyDown("o"))
        {
            EventManager.TriggerEvent("Spawn");
        }
        if (Input.GetKeyDown("p"))
        {
            EventManager.TriggerEvent("Destroy");
        }
        if (Input.GetKeyDown("x"))
        {
            EventManager.TriggerEvent("Junk");
        }
        if (Input.GetKeyDown("d"))
        {
            EventManager.TriggerEvent("Debug");
        }
        if (Input.GetKeyDown(KeyCode.Escape))
        {
           if (menu.isActiveAndEnabled)
            { menu.enabled = false; }
           else { menu.enabled = true; }
        }
    }
}
