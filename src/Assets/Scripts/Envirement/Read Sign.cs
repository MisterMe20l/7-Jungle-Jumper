using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class ReadSign : MonoBehaviour
{
    GameInput gameInput;

    [SerializeField] Animator m_anmIcon;
    [SerializeField] Animator m_anmCapsule;

    [SerializeField] Transform m_readingRange;
    [SerializeField] LayerMask m_playerLayer;

    private bool showingIcon = false;
    private bool showingCapsule = false;
    private InputAction interact;

    private void Awake()
    {
        gameInput = new GameInput(); 
    } 

    private void OnEnable()
    {
        interact = gameInput.Player.Interact;
        interact.Enable();
        interact.performed += OnInteractInput;
    }

    private void OnDisable()
    {
        interact.Disable();
    }

    private void UpdateAnimator()
    {
        m_anmIcon.SetBool("Showing", showingIcon);
        m_anmCapsule.SetBool("Showing", showingCapsule);
    }

    private void Update()
    {
        UpdateAnimator();
        Showing();
    }

    private void Showing()
    {
        var readable = OnReadingRange();
        if (!readable) // Out of range
        {
            showingIcon = false;
            showingCapsule = false;
        }
        else if (readable && !showingIcon && !showingCapsule) // On range
        {
            showingIcon = true;
        }
    }

    private void OnInteractInput(InputAction.CallbackContext context)
    {
        if (showingIcon)
        {
            showingIcon = false;
            showingCapsule = true;
        }
        else if (showingCapsule)
        {
            showingCapsule = false;
            showingIcon = true;
        }
    }

    private bool OnReadingRange()
    {
        return Physics2D.OverlapBox(m_readingRange.position, 
            m_readingRange.lossyScale, 0, m_playerLayer) != null;
    }
}
