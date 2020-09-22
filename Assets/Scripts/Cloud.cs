using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Cloud : MonoBehaviour
{
    public float windSpeed = 1f;
    public Vector3 windDirection;

    void Update()
    {
        transform.position = Vector3.MoveTowards(transform.position, transform.position + windDirection, windSpeed * Time.deltaTime);
    }
}
