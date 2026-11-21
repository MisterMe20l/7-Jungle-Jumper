using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OnClick : MonoBehaviour
{
    [SerializeField] AudioSource m_clickSound;

    public void PlayClickSound()
    {
        m_clickSound.Play();
    }
}
