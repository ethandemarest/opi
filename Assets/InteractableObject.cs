using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InteractableObject : MonoBehaviour
{
    public Rigidbody2D rb;
    public GameObject opi;
    Animator itemAnim;
    Animator opiAnim;

    Vector3 itemPosition;
    Vector3 opiPosition;
    Vector3 offest;
    Vector3 toss;

    private int focus;
    private float smoothSpeed = 1f;
    public bool held;

    float lastMoveX;
    float lastMoveY;

    float speedUpDown = 3f;
    float distanceUpDown = 0.2f;

    void Start()
    {
        focus = 0;
        itemAnim = this.GetComponent<Animator>();
        opiAnim = opi.GetComponent<Animator>();
        held = false;
    }

    void FixedUpdate()
    {
        //HOVER
        if(focus == 0)
        {
            Vector3 mov = new Vector3(transform.position.x, Mathf.Sin(speedUpDown * Time.time) * distanceUpDown, transform.position.z);
            transform.position = mov;
        }

        itemPosition = this.gameObject.GetComponent<Transform>().position;
        opiPosition = GameObject.Find("Opi").GetComponent<Transform>().position;
        offest = new Vector3(0f, 2.5f, 0f);

        Vector3[] targetPosition = new Vector3[3];
        targetPosition[0] = itemPosition; 
        targetPosition[1] = opiPosition + offest;
        targetPosition[2] = toss;

        rb.MovePosition(Vector3.Lerp(itemPosition, targetPosition[focus], smoothSpeed));

    }


    // HELD STATUS
    public void DoInteraction1() //Picked Up
    {
        if (held == false)
        {
            focus = 1;
            smoothSpeed = 2f;
            opiAnim.SetBool("Item", true);
        }
    }
    public void DoInteraction2() //Dropped
    {
        lastMoveX = opi.GetComponent<PlayerController>().lastMoveX * 3;
        lastMoveY = opi.GetComponent<PlayerController>().lastMoveY * 3;

        toss = itemPosition + new Vector3(lastMoveX, lastMoveY, 0f);
        focus = 2;
        smoothSpeed = 0.1f;
        opiAnim.SetBool("Item", false);
        held = true;

        StartCoroutine("coRoutineTest");
    }

    public void DoInteraction3() //Cauldron Delivery
    {
        itemAnim.SetBool("Submit", true);
    }

    IEnumerator coRoutineTest()
    {
        yield return new WaitForSeconds(1f);
        held = false;
    }

  


}
    