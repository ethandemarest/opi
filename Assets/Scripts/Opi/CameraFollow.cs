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
        speed = 0.4f;
        opi = GameObject.Find("Opi");
        transform.position = opi.transform.position + introOffset;
        playerController = opi.GetComponent<PlayerController>();
        angleNumber = 0;
    }

    private void FixedUpdate()
    {
        //Camera Positions

        Vector3[] newAngles = new Vector3[3];
        newAngles[0] = new Vector3(playerController.lastMove.x * leadFactor, playerController.lastMove.y * leadFactor , -50) + opi.transform.position;
        newAngles[1] = newAngle;
        newAngles[2] = new Vector3(opi.transform.position.x, opi.transform.position.y, -50);



        //Zoom 
        float[] zoom = new float[3]; 
        zoom[0] = defaultZoom + (Mathf.Clamp(playerController.movement.sqrMagnitude, 0, 1) * zoomFactor); 
        zoom[1] = cameraZoom;
        zoom[2] = 5;


        //Position Smooth
        Vector3 smoothedPosition = Vector3.SmoothDamp(transform.position, newAngles[angleNumber], ref velocity, speed);
        transform.position = smoothedPosition;

        //Zoom Smooth
        mainCamera.orthographicSize = Mathf.SmoothDamp(mainCamera.orthographicSize, zoom[angleNumber], ref velocityY, speed);

    }

    public void CameraTrigger(Vector3 angle, int zoom, float zoomSpeed)
    {
        angleNumber = 1;
        newAngle = angle;
        cameraZoom = zoom;
        speed = zoomSpeed;
    }
    void DeathAngle()
    {
        print("death angle");
        angleNumber = 2;
    }
}
