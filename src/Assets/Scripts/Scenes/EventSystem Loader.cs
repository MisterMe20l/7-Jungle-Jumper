using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;

public class EventSystemLoader : MonoBehaviour
{
    EventSystem eventSystem;
    [SerializeField] int m_sceneCountToEnable;

    private void Awake()
    {
        eventSystem = GetComponent<EventSystem>();
    }

    private void Update()
    {
        if (!eventSystem.enabled && SceneManager.loadedSceneCount == m_sceneCountToEnable)
        {
            StartCoroutine(ActivateEventSystem());
        }
        else if (eventSystem.enabled && SceneManager.loadedSceneCount == m_sceneCountToEnable + 1)
        {
            eventSystem.enabled = false;
        }
    }

    private IEnumerator ActivateEventSystem()
    {
        yield return new WaitForEndOfFrame();
        yield return new WaitForEndOfFrame();
        yield return new WaitForEndOfFrame();
        yield return new WaitForEndOfFrame();

        eventSystem.enabled = true;
    }
}
