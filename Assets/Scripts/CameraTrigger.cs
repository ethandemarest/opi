using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraTrigger : MonoBehaviour
{
    public int sceneAngle;

    void OnTriggerEnter2D(Collider2D collider2D)
    {
        if (collider2D.CompareTag("Opi"))
        {
            GameObject.Find("Main Camera").GetComponent<CameraFollow>().angle = sceneAngle;

        }
    }

    //Exit Trigger
    void OnTriggerExit2D(Collider2D collider2D)
    {
        if (collider2D.CompareTag("Opi"))
        {
            GameObject.Find("Main Camera").GetComponent<CameraFollow>().angle = 0;

        }
    }


}
