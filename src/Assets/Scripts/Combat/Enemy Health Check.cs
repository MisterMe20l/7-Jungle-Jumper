using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyHealthCheck : MonoBehaviour
{
    GameObject[] objects;
    [SerializeField] Enemy enemy1;

    private void Start()
    {
        objects = new GameObject[2];

        objects[0] = GameObject.FindGameObjectWithTag("ExitPortal");
        objects[1] = GameObject.FindGameObjectWithTag("WorldSound");
    }

    private void Update()
    {
        if (enemy1.alive && enemy1.m_health <= 0)
        {
            enemy1.alive = false;
            OnEnemyDeath();
        }
    }

    private void OnEnemyDeath()
    {
        for (int i = 0; i < objects.Length; i++)
            objects[i].GetComponent<IWinGame>().WinGame();
    }
}
