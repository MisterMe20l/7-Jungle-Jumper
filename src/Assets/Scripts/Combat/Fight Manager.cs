using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FightManager : MonoBehaviour
{
    GameObject[] objects;

    [SerializeField] Player player1;
    [SerializeField] LayerMask m_playerLayer;

    bool started = false;

    private void Start()
    {
        objects = new GameObject[4];
        objects[0] = GameObject.FindGameObjectWithTag("Player");
        objects[1] = GameObject.FindGameObjectWithTag("Enemy");
        objects[2] = GameObject.FindGameObjectWithTag("DoorSpawnpoint");
        objects[3] = GameObject.FindGameObjectWithTag("WorldSound");

    }

    private void Update()
    {
        if (!started && PlayerCheck())
        {
            started = true;
            for (int i = 0; i < objects.Length; i++)
                objects[i].GetComponent<IStartFight>().StartFight();
        }

        if (started && PlayerDead())
        {
            started = false;
            for (int i = 0; i < objects.Length; i++)
                objects[i].GetComponent<IStartFight>().RestartFight();
        }
    }

    private bool PlayerCheck()
    {
        return Physics2D.OverlapBox(transform.position,
            transform.lossyScale, 0, m_playerLayer) != null;
    }

    private bool PlayerDead()
    {
        return player1.health <= 0;
    }
}
