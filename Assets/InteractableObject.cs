using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InteractableObject : MonoBehaviour
{
    public Rigidbody2D rb;
    public GameObject opi;

    Vector3 itemPosition;
    Vector3 opiPosition;
    Vector3 offest;

    public int focus = 0;
    public float smoothSpeed = 1;


    public void DoInteraction1()
    {
        //gameObject.SetActive (false);
        focus = 1;
        print("Interaction1");

    }
    public void DoInteraction2()
    {
        //gameObject.SetActive (false);
        focus = 0;
        print("Interaction2");

    }

    void Update()
    {
        itemPosition = this.gameObject.GetComponent<Transform>().position;
        opiPosition = GameObject.Find("Opi").GetComponent<Transform>().position;
        offest = new Vector3(0f, 2.5f, 0f);


        Vector3[] targetPosition = new Vector3[2];
        targetPosition[0] = itemPosition; 
        targetPosition[1] = opiPosition + offest;


        rb.MovePosition(Vector3.Lerp(itemPosition, targetPosition[focus], smoothSpeed));
        
    }

}
   