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

    //Alt Positions


    private void Start()
    {
        cam = Camera.main;
    }


    private void FixedUpdate()
    {

        Vector3 desiredPosition = target.position + offset;

        // Position Smooth
        Vector3 smoothedPosition = Vector3.Lerp(transform.position, desiredPosition, smoothSpeed);
        transform.position = smoothedPosition;

        
        //Zoom Smooth
        //cam.orthographicSize = Mathf.Lerp(cam.orthographicSize, targetZoom, smoothSpeed);
        
    }
    
}
