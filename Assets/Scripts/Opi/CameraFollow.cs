using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    public Camera mainCamera;

    GameObject opi;
    GameObject wizard;
    PlayerController playerController;

    //Zoom Fields
    public float defaultZoom = 10;
    
    //Position Fields
    public float smoothSpeed = 0.125f;
    public float leadFactor = 5;
    public float zoomFactor = 1;
    public Vector3 offset;
    public Vector3 introOffset;
    public int angle;
    public int speed;

    private Vector3 velocity = Vector3.zero;    
    private float velocityY = 0f;

    private void Start()
    {
        opi = GameObject.Find("Opi");
        wizard = GameObject.Find("Wizard");
        transform.position = opi.transform.position + introOffset;
        playerController = opi.GetComponent<PlayerController>();
        angle = 0;
    }

    private void FixedUpdate()
    {
        //Camera Positions

        Vector3[] newAngles = new Vector3[5];
        newAngles[0] = new Vector3(playerController.movement.x * leadFactor, playerController.movement.y * leadFactor, 0) + opi.transform.position + offset; //Opi
        newAngles[1] = new Vector3(wizard.transform.position.x, wizard.transform.position.y, -80f); //Witch
        newAngles[2] = new Vector3(5f, 15f, -80f); 
        newAngles[3] = new Vector3(-13f, 9f, -50f); //Tree
        newAngles[4] = opi.transform.position + introOffset; //Intro

        //Zoom 
        float[] zoom = new float[5]; 
        zoom[0] = defaultZoom + (Mathf.Clamp(playerController.movement.sqrMagnitude, 0, 1) * zoomFactor); 
        zoom[1] = 10; //Witch
        zoom[2] = 10; //Witch
        zoom[3] = 20; //Tree
        zoom[4] = 8; //Intro

        //Smooth Speed
        float[] newSpeed = new float[2];
        newSpeed[0] = 1.8f; //slow
        newSpeed[1] = 0.5f; //default

        //Position Smooth
        Vector3 smoothedPosition = Vector3.SmoothDamp(transform.position, newAngles[angle], ref velocity ,newSpeed[speed]);
        transform.position = smoothedPosition;

        //Zoom Smooth
        mainCamera.orthographicSize = Mathf.SmoothDamp(mainCamera.orthographicSize, zoom[angle], ref velocityY, newSpeed[speed]);

    }
    
}
