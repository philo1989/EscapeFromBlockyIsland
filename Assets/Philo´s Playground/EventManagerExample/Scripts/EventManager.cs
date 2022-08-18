using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class EventManager : MonoBehaviour
{
    private Dictionary<string, UnityEvent> eventDictionary;

    private static EventManager eventManager; // private static instance of EventManager

    public static EventManager instance // getter for the eventManager instance called instance
    {
        get
        {
            if (!eventManager)
            {
                eventManager = FindObjectOfType(typeof(EventManager)) as EventManager;///WHAAAA?????

                if (!eventManager)
                {
                    Debug.LogError("There needs to be one active EventManager script on a gameObject in your scene");
                }
                else
                {
                    eventManager.Init();
                }
            }
            return eventManager;
        }
    }

    void Init()
    {
        if (eventDictionary == null)
        {
            eventDictionary = new Dictionary<string, UnityEvent>();
        }
    }
    //listener ist der code der "horcht" ob das event getriggert wurde und sich ausführtt wenn ja
    public static void StartListening(string eventName, UnityAction listener)
    {
        UnityEvent thisEvent = null;
        if (instance.eventDictionary.TryGetValue(eventName, out thisEvent))
        {
            thisEvent.AddListener(listener);
        }
        else
        {
            thisEvent = new UnityEvent();
            thisEvent.AddListener(listener);
            instance.eventDictionary.Add(eventName, thisEvent);
        }
    }

    public static void StopListening(string eventName, UnityAction listener)
    {
        if (eventManager == null) return;
        UnityEvent thisEvent = null;
        if (instance.eventDictionary.TryGetValue(eventName, out thisEvent))
        {
            thisEvent.RemoveListener(listener);
            // MAyBE adding afeedback to anounce succes
        }
    }

    public static void TriggerEvent(string eventName)
    {
        UnityEvent thisEvent = null;
        if (instance.eventDictionary.TryGetValue(eventName, out thisEvent))
        {
            thisEvent.Invoke();
        }
        //else if (eventName == "Debug")
        //{
        //    Debug.Log(instance.eventDictionary.Keys);
        //    Debug.Log(instance.eventDictionary.Values);
            
        //}
        else if (!instance.eventDictionary.ContainsKey(eventName))
        {
            Debug.LogWarning("something tried to trigger the event: " + eventName + "\n\r however this event isn´t registered");
        }
        
    }
}

