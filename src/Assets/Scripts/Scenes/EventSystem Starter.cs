using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class EventSystemStarter : MonoBehaviour
{
    EventSystem eventSystem;

    private void Awake()
    {
        eventSystem = GetComponent<EventSystem>();
    }

    private void Start()
    {
        StartCoroutine(ActivateEventSystem());
    }

    private IEnumerator ActivateEventSystem()
    {
        yield return new WaitForEndOfFrame();

        eventSystem.enabled = true;
    }
}
