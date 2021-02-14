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
    public Vector3 introOffset;
    public Vector3 newAngle;
    public int angleNumber;

    float speed;
    Vector3 velocity = Vector3.zero;    
    float velocityY = 0f;

    private void Start()
    {
        speed = 0.5f;
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

        //Position Smooth
        Vector3 smoothedPosition = Vector3.SmoothDamp(transform.position, newAngles[angleNumber], ref velocity ,speed);
        transform.position = smoothedPosition;

        //Zoom Smooth
        mainCamera.orthographicSize = Mathf.SmoothDamp(mainCamera.orthographicSize, zoom[angleNumber], ref velocityY, speed);

    }

    public void CameraTrigger(Vector3 angle, int zoom, float zoomSpeed)
    {
        newAngle = angle;
        cameraZoom = zoom;
        speed = zoomSpeed;
    }
}
