using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InteractableObject : MonoBehaviour
{
    private Rigidbody2D rb;
    Animator itemAnim;

    private GameObject opi; 
    Animator opiAnim;

    private GameObject shadow;
    Vector3 shadowScale;

    Vector3 itemPosition;
    Vector3 opiPosition;
    Vector3 offest;
    Vector3 toss;

    private int itemStatus;
    private float smoothSpeed = 1f;
    public bool held;

    private float speed;

    float lastMoveX;
    float lastMoveY;

    //float speedUpDown = 3f;
    //float distanceUpDown = 0.2f;

    void Start()
    {
        itemStatus = 0;
        opi = GameObject.Find("Opi");
        rb = this.GetComponent<Rigidbody2D>();

        shadow = this.transform.GetChild(0).gameObject;
        

        itemAnim = this.GetComponent<Animator>();
        opiAnim = opi.GetComponent<Animator>();
        held = false;
    }

    void FixedUpdate()
    {

        shadowScale = shadow.transform.localScale;

        speed = opi.GetComponent<PlayerController>().movement.sqrMagnitude;

        itemPosition = this.gameObject.GetComponent<Transform>().position;
        opiPosition = GameObject.Find("Opi").GetComponent<Transform>().position;
        offest = new Vector3(0f, 2.5f, 0f);

        Vector3[] targetPosition = new Vector3[3];
        targetPosition[0] = itemPosition; 
        targetPosition[1] = opiPosition + offest;
        targetPosition[2] = toss;

        rb.MovePosition(Vector3.Lerp(itemPosition, targetPosition[itemStatus], smoothSpeed));


        //BOUNCE ANIMATION
        if(itemStatus == 1 && speed > 0.1)
        {
            itemAnim.SetBool("Opi Walking", true);
        }
        else
        {
            itemAnim.SetBool("Opi Walking", false);
        }

    }


    // HELD STATUS
    public void DoInteraction1() //Picked Up
    {
        if (held == false)
        {
            itemStatus = 1;
            smoothSpeed = 2f;
            opiAnim.SetBool("Item", true);

            shadow.SetActive(false);

        }
    }
    public void DoInteraction2() //Dropped
    {
        lastMoveX = opi.GetComponent<PlayerController>().lastMoveX * 3;
        lastMoveY = opi.GetComponent<PlayerController>().lastMoveY * 3;
        toss = itemPosition + new Vector3(lastMoveX, lastMoveY - 3, 0f);

        itemStatus = 2;
        smoothSpeed = 0.1f;
        opiAnim.SetBool("Item", false);
        held = true;

        StartCoroutine("coRoutineTest");
        itemAnim.SetBool("Item Drop", true);

        shadow.SetActive(true);
        shadowScale = Vector3.Lerp(shadowScale*0, shadowScale, 0.01f);
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
    