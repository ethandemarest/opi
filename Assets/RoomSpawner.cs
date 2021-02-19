using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoomSpawner : MonoBehaviour
{
    public GameObject[] enemy;
    public float spawnDelay;

    private void OnTriggerEnter2D(Collider2D collision)
    {

        if (collision.CompareTag("Opi"))
        {
            print("2D");
            StartCoroutine("Spawn");

        }
    }

    IEnumerator Spawn()
    {
        yield return new WaitForSeconds(spawnDelay);

        print("spawn");

        foreach (GameObject s in enemy)
        {
            s.SetActive(true);
        }
    }

}


