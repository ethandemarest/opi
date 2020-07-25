using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraTrigger : MonoBehaviour
{
    public int sceneAngle;
    

    //Enter Trigger
    public void OnTriggerEnter2D(Collider2D collision)
    {
        GameObject.Find("Main Camera").GetComponent<CameraFollow>().angle = sceneAngle;
    }

    //Exit Trigger
    public void OnTriggerExit2D(Collider2D collision)
    {
        GameObject.Find("Main Camera").GetComponent<CameraFollow>().angle = 0;
    }
}
