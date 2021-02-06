using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InteractableObject : MonoBehaviour
{
    //ITEM
    Rigidbody2D rb;
    Animator itemAnim;
    BoxCollider2D itemCollider;

    //OPI
    GameObject opi; 
    Animator opiAnim;

    //SHADOW
    public GameObject shadow;

    Vector3 toss;

    public int holder = 0;
    public int itemStatus = 0;
    public float throwDistance = 4;
    float smoothSpeed = 1f;
    float speed;
    float lastMoveX;
    float lastMoveY;

    void Start()
    {
        opi = GameObject.Find("Opi");
        opiAnim = opi.GetComponent<Animator>();
        rb = this.GetComponent<Rigidbody2D>();
        itemCollider = this.GetComponent<BoxCollider2D>();
        itemAnim = this.GetComponent<Animator>();
        
    }

    void Update()
    {
        speed = opi.GetComponent<PlayerController>().movement.sqrMagnitude;


        //Positions
        if (holder != 0)
        {
            Vector3[] targetPosition = new Vector3[3];
            targetPosition[0] = transform.position;
            targetPosition[1] = opi.transform.position + new Vector3(0f, 2.5f, 0f);
            targetPosition[2] = toss;

            //TOSS
            if (itemStatus == 2)
            {
                rb.MovePosition(Vector3.Lerp(transform.position, targetPosition[itemStatus], smoothSpeed));
            }
            else
            {
                rb.MovePosition(targetPosition[itemStatus]);
            }

            //BOUNCE ANIMATION
            if (itemStatus == 1 && speed > 0.1)
            {
                itemAnim.SetBool("Opi Walking", true);
            }
            else
            {
                itemAnim.SetBool("Opi Walking", false);
            }
        }     
    }


    // OPI PICKED UP
    public void OpiPickUp()
    {
        holder = 1;
        itemStatus = 1;

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
        lastMoveX = opi.GetComponent<PlayerController>().lastMove.x * throwDistance;
        lastMoveY = opi.GetComponent<PlayerController>().lastMove.y * throwDistance;
        toss = transform.position + new Vector3(lastMoveX, lastMoveY - 3, 0f);
        
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
        opiAnim.SetBool("Scene Trigger", true);
        StartCoroutine("destroyObj");
    }

    IEnumerator dropped()
    {
        yield return new WaitForSeconds(0.5f);

        itemCollider.enabled = !itemCollider.enabled;
        holder = 0;
    }

    IEnumerator destroyObj()
    {
        yield return new WaitForSeconds(1f);

        Destroy(this.gameObject);
    }



}
    