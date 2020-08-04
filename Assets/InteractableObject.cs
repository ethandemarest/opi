using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InteractableObject : MonoBehaviour
{
    private Rigidbody2D rb;
    Animator itemAnim;
    BoxCollider2D itemCollider;

    private GameObject opi; 
    Animator opiAnim;

    private GameObject shadow;

    Vector3 itemPosition;
    Vector3 opiPosition;
    Vector3 offest;
    Vector3 toss;

    private int itemStatus;
    private float smoothSpeed = 1f;

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
        itemCollider = this.GetComponent<BoxCollider2D>();

        shadow = this.transform.GetChild(0).gameObject;
        

        itemAnim = this.GetComponent<Animator>();
        opiAnim = opi.GetComponent<Animator>();
    }

    void FixedUpdate()
    {
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


    // PICKED UP
    public void DoInteraction1()
    {
        itemStatus = 1;
        smoothSpeed = 2f;
        opiAnim.SetBool("Item", true);

        // SHADOW
        shadow.SetActive(false);
    }

    //DROPPED
    public void DoInteraction2()
    {
        itemCollider.enabled = !itemCollider.enabled;

        //THROW ANIMATION
        itemStatus = 2;
        smoothSpeed = 0.1f;
        lastMoveX = opi.GetComponent<PlayerController>().lastMoveX * 3;
        lastMoveY = opi.GetComponent<PlayerController>().lastMoveY * 3;
        toss = itemPosition + new Vector3(lastMoveX, lastMoveY - 3, 0f);
        
        opiAnim.SetBool("Item", false);
        itemAnim.SetBool("Item Drop", true);

        StartCoroutine("dropped");

        //SHADOW
        shadow.SetActive(true);
    }

    //CAULDRON DELIVERY
    public void DoInteraction3() 
    {
        itemAnim.SetBool("Submit", true);
        opiAnim.SetBool("Item", false);
        StartCoroutine("destroyObj");
    }

    IEnumerator dropped()
    {
        yield return new WaitForSeconds(3f);

        print("recharge");
        itemCollider.enabled = !itemCollider.enabled;


    }

    IEnumerator destroyObj()
    {
        yield return new WaitForSeconds(1f);

        Destroy(this.gameObject);
    }



}
    