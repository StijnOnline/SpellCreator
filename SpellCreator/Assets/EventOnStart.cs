using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EventOnStart : MonoBehaviour
{
    public SpellCreator.Event Event;


    public void Start()
    {
        if(Event != null)
            StartCoroutine(Event.ExecuteCoRoutine(gameObject));
        else
            Debug.LogWarning("EventOnStart: Event isn't set");
    }
}
