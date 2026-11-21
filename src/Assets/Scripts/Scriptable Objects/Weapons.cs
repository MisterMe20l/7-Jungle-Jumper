using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu]
public class Weapons : ScriptableObject
{
    [SerializeField] public string m_name;
    [SerializeField] public int m_damage;
    [SerializeField] public float m_firingRate_s;
    [SerializeField] public float m_projectileSpeed;
    [SerializeField] public GameObject m_bulletPrefab;
}
