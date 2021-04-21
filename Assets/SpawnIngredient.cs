using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnIngredient : MonoBehaviour
{
    public GameObject spawner;
    public GameObject ingredient;
    public Vector3 offset;
    public float delay;

    IEnumerator SpawnItem()
    {
        yield return new WaitForSeconds(delay);
        Instantiate(ingredient, spawner.transform.position + offset, Quaternion.Euler(0f, 0f, 0f));
    }
}
