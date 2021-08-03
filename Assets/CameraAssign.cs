using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraAssign : MonoBehaviour
{
    Canvas canvas;

    void Start()
    {
        canvas = GetComponent<Canvas>();
        canvas.worldCamera = Camera.main;
    }
}
