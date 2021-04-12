using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class opiFollow : MonoBehaviour
{
    public GameObject parent;
    public Vector3 offset;

    void FixedUpdate()
    {
        transform.position = parent.transform.position + offset * Time.deltaTime;
    }
}
