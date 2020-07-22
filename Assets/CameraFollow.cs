using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    //Zoom Fields
    private Camera cam;
    public float defaultZoom = 8;
    public float zoomAmount = 20;
    
    

    //Position Fields
    public Transform target;
    public float smoothSpeed = 0.125f;
    public Vector3 offset;
    public PlayerController opiScript;

    //Alt Positions
    public Vector3 WitchCamera;
    public bool camTrigger;


    private void Start()
    {
        cam = Camera.main;
    }


    private void FixedUpdate()
    {
        camTrigger = GameObject.Find("Opi").GetComponent<PlayerInteract>().cameraTrigger;
        Vector3 desiredPosition = target.position + offset;

        float targetZoom;

        //Trigger Check
        if(camTrigger == true)
        {
            targetZoom = zoomAmount;

            desiredPosition = WitchCamera;

        }
        else
        {
            targetZoom = defaultZoom;
        }

        // Position Smooth
        Vector3 smoothedPosition = Vector3.Lerp(transform.position, desiredPosition, smoothSpeed);
        transform.position = smoothedPosition;

        
        //Zoom Smooth
        cam.orthographicSize = Mathf.Lerp(cam.orthographicSize, targetZoom, smoothSpeed);
        
    }
    
}
