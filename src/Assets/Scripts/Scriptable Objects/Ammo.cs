using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu]
public class Ammo : ScriptableObject
{
    [SerializeField] public LayerMask m_targetLayer;
    [SerializeField] public GameObject m_target;
}
