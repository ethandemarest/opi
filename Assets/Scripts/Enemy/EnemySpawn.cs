using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawn : MonoBehaviour
{

    public GameObject enemy;
    GameObject opi;
    PlayerController pc;
    Vector2 spawn;

    void Start()
    {
        opi = GameObject.Find("Opi");
        pc = opi.GetComponent<PlayerController>();
    }

    void Update()
    {
        if (Input.GetButtonDown("interact"))
        {
            Spawn();
        }
    }

    public void Spawn()
    {
        spawn.x = opi.transform.position.x + pc.lastMove.x * 8;
        spawn.y = opi.transform.position.y + pc.lastMove.y * 8;
        Instantiate(enemy, spawn, Quaternion.Euler(0f, 0f, 0f));
    }
}
