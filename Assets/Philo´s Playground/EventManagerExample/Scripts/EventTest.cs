using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class EventTest : MonoBehaviour
{
    private UnityAction someListener;

    private void Awake()
    {
        someListener = new UnityAction(SomeFunction);
    }

    private void OnEnable()
    {
        EventManager.StartListening("test", someListener);
        //EventManager.StartListening("Spawn", SomeOtherFunction);
        //EventManager.StartListening("Destroy", SomeThirdFunction);
    }
    private void OnDisable()
    {
        EventManager.StopListening("test", someListener);
        //EventManager.StopListening("Spawn", SomeOtherFunction);
        //EventManager.StopListening("Destroy", SomeThirdFunction);
    }
    void SomeFunction()
    {
        Debug.Log("SomeFunction()");
    }
    void SomeOtherFunction()
    {
        Debug.Log("SomeOtherFunction()");
    }void SomeThirdFunction()
    {
        Debug.Log("SomeThirdFunction()");
    }

}
