using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Foreground : MonoBehaviour
{
    public float layer;

    void Update()
    {
        transform.position = transform.position - (Camera.main.transform.position * layer);
    }
}
