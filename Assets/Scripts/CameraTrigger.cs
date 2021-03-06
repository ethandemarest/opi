﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraTrigger : MonoBehaviour
{
    CameraFollow cameraFollow;
    public Vector3 angle;
    public int zoom;
    public float speed;

    public void Start()
    {
        cameraFollow = GameObject.Find("Camera Holder").GetComponent<CameraFollow>();
    }

    void OnTriggerEnter2D(Collider2D collider2D)
    {
        if (collider2D.CompareTag("Opi"))
        {
            cameraFollow.angleNumber = 1;
            cameraFollow.CameraTrigger(angle, zoom, speed);
        }
    }

    //Exit Trigger
    void OnTriggerExit2D(Collider2D collider2D)
    {
        if (collider2D.CompareTag("Opi"))
        {
            cameraFollow.angleNumber = 0;
        }
    }


}
