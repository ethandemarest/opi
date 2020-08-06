﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    public Camera mainCamera;

    GameObject opi;
    PlayerController playerController;

    //Zoom Fields
    public float defaultZoom = 5;

    //Position Fields
    Transform target;
    public float smoothSpeed = 0.125f;
    public Vector3 offset;
    public int angle;

    private void Start()
    {
        opi = GameObject.Find("Opi");
        playerController = opi.GetComponent<PlayerController>();
    }


    private void FixedUpdate()
    {
        //Camera Positions

        target = opi.transform;

        Vector3[] newAngles = new Vector3[4];
        newAngles[0] = new Vector3(playerController.inputX*5, playerController.inputY*5, 0) + opi.transform.position + offset; //Opi
        newAngles[1] = new Vector3(5f, 15f, -50f); //Witch
        newAngles[2] = new Vector3(5f, 12f, -50f); 
        newAngles[3] = new Vector3(-13f, 9f, -50f); //Tree


        //Zoom 
        float[] zoom = new float[4]; 
        zoom[0] = defaultZoom + playerController.movement.sqrMagnitude; //Opi
        zoom[1] = 10; //Witch
        zoom[2] = 10; //Witch
        zoom[3] = 20; //Tree



        // Position Smooth
        Vector3 desiredPosition = newAngles[angle];

        Vector3 smoothedPosition = Vector3.Lerp(transform.position, desiredPosition, smoothSpeed);
        transform.position = smoothedPosition;


        //Zoom Smooth
        float targetZoom = zoom[angle];

        mainCamera.orthographicSize = Mathf.Lerp(mainCamera.orthographicSize, targetZoom, smoothSpeed+smoothSpeed);
        
    }
    
}
