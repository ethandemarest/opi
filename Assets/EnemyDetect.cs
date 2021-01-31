using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyDetect : MonoBehaviour
{
    public GameObject otherEnemy;
    public Vector2 otherEnemyPosition;

    // Update is called once per frame
    void Update()
    {
        if (otherEnemy != null)
        {
            otherEnemyPosition = otherEnemy.transform.position;
        }
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Enemy"))
        {
            otherEnemy = other.gameObject;
        }
    }
    void OnTriggerExit2D(Collider2D other)
    {
        otherEnemy = null;
    }
}
