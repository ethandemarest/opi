using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InteractableObject : MonoBehaviour
{
    public Rigidbody2D rb;
    public GameObject opi;
    Animator animator;

    Vector3 itemPosition;
    Vector3 opiPosition;
    Vector3 offest;
    Vector3 toss;

    public int focus;
    public float smoothSpeed = 1f;
    public bool held;

    float lastMoveX;

    void Start()
    {
        focus = 0;
        animator = opi.GetComponent<Animator>();
    }

    public void DoInteraction1() //Picked Up
    {
        
        focus = 1;
        smoothSpeed = 0.5f;
        held = true;
        animator.SetBool("Item", true);

    }
    public void DoInteraction2() //Dropped
    {
        
        lastMoveX = opi.GetComponent<PlayerController>().lastMoveX * 3;
      

        toss = itemPosition + new Vector3(lastMoveX, -3, 0f);
        focus = 2;
        smoothSpeed = 0.1f;
        animator.SetBool("Item", false);

        

        
    }

    void FixedUpdate()
    {
        //HOVER
        /*
        Vector3 mov = new Vector3(transform.position.x, Mathf.Sin(speedUpDown * Time.time) * distanceUpDown, transform.position.z);
        transform.position = mov;
        */

        itemPosition = this.gameObject.GetComponent<Transform>().position;
        opiPosition = GameObject.Find("Opi").GetComponent<Transform>().position;
        offest = new Vector3(0f, 2.5f, 0f);


        Vector3[] targetPosition = new Vector3[3];
        targetPosition[0] = itemPosition; 
        targetPosition[1] = opiPosition + offest;
        targetPosition[2] = toss;


        rb.MovePosition(Vector3.Lerp(itemPosition, targetPosition[focus], smoothSpeed));

        



    }

  


}
    