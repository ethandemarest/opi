using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    public Camera mainCamera;

    GameObject opi;
    PlayerController playerController;

    //Zoom Fields
    public float defaultZoom = 10;
    float cameraZoom;
    
    //Position Fields
    float leadFactor = 3;
    float zoomFactor = 1;
    Vector3 offset;
    public Vector3 introOffset;
    public Vector3 newAngle;
    public int angleNumber;
    public int speed;

    Vector3 velocity = Vector3.zero;    
    float velocityY = 0f;

    private void Start()
    {
        opi = GameObject.Find("Opi");
        transform.position = opi.transform.position + introOffset;
        playerController = opi.GetComponent<PlayerController>();
        angleNumber = 0;
    }

    private void FixedUpdate()
    {
        //Camera Positions

        Vector3[] newAngles = new Vector3[2];
        newAngles[0] = new Vector3(playerController.lastMove.x * leadFactor, playerController.lastMove.y * leadFactor , -50) + opi.transform.position;
        newAngles[1] = newAngle;


        //Zoom 
        float[] zoom = new float[2]; 
        zoom[0] = defaultZoom + (Mathf.Clamp(playerController.movement.sqrMagnitude, 0, 1) * zoomFactor); 
        zoom[1] = cameraZoom;


        //Smooth Speed
        float[] newSpeed = new float[2];
        newSpeed[0] = 1.8f; //slow
        newSpeed[1] = 0.5f; //default

        //Position Smooth
        Vector3 smoothedPosition = Vector3.SmoothDamp(transform.position, newAngles[angleNumber], ref velocity ,newSpeed[speed]);
        //Vector3 smoothedPosition = Vector3.MoveTowards(transform.position, newAngles[angleNumber], newSpeed[speed]);
        transform.position = smoothedPosition;

        //Zoom Smooth
        mainCamera.orthographicSize = Mathf.SmoothDamp(mainCamera.orthographicSize, zoom[angleNumber], ref velocityY, newSpeed[speed]);

    }

    public void CameraTrigger(Vector3 angle, float zoom)
    {
        newAngle = angle;
        cameraZoom = zoom;
    }
    
}
