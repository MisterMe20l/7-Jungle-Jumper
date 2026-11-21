using UnityEngine;

[CreateAssetMenu]
public class Enemy : ScriptableObject
{
    [SerializeField] public int m_health = 100;
    public bool awake = false;
    public bool alive = true;
}