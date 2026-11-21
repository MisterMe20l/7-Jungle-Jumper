using UnityEngine;

[CreateAssetMenu]
public class Player : ScriptableObject
{
    [SerializeField] public int health = 100;
    [SerializeField] public string currentWeapon;
    public GameObject currentCheckpoint;
    public bool waiting;
    public bool canShoot;
}
