using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InteractableObject : MonoBehaviour
{

    //ITEM
    private Rigidbody2D rb;
    Animator itemAnim;
    BoxCollider2D itemCollider;

    //OPI
    private GameObject opi; 
    Animator opiAnim;

    //ENEMY
    private GameObject enemy;

    //SHADOW
    private GameObject shadow;

    Vector3 itemPosition;
    Vector3 opiPosition;
    Vector3 offset;
    Vector3 toss;

    public int holder = 0;
    public int itemStatus = 0;
    float smoothSpeed = 1f;
    float speed;
    float lastMoveX;
    float lastMoveY;

    void Start()
    {
        opi = GameObject.Find("Opi");
        opiAnim = opi.GetComponent<Animator>();

        enemy = GameObject.Find("Enemy");

        rb = this.GetComponent<Rigidbody2D>();
        itemCollider = this.GetComponent<BoxCollider2D>();
        itemAnim = this.GetComponent<Animator>();
        
        shadow = this.transform.GetChild(0).gameObject;
    }

    void FixedUpdate()
    {
        speed = opi.GetComponent<PlayerController>().movement.sqrMagnitude;

        itemPosition = this.gameObject.GetComponent<Transform>().position;
        opiPosition = GameObject.Find("Opi").GetComponent<Transform>().position;
        offset = new Vector3(0f, 2.5f, 0f);

        //Game Objects
        GameObject[] targetObject = new GameObject[3];
        targetObject[0] = null;
        targetObject[1] = opi;
        targetObject[2] = enemy;

        //Positions
        if (holder != 0)
        {
            Vector3[] targetPosition = new Vector3[3];
            targetPosition[0] = itemPosition;
            targetPosition[1] = targetObject[holder].GetComponent<Transform>().position + offset;
            targetPosition[2] = toss;

            //TOSS
            if (itemStatus == 2)
            {
                rb.MovePosition(Vector3.Lerp(itemPosition, targetPosition[itemStatus], smoothSpeed));
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

    // ENEMY PICKED UP
    public void EnemyPickUp()
    {
        holder = 2;
        itemStatus = 1;
       
        // SHADOW
        shadow.SetActive(false);
    }

    // ENEMY DROP
    public void EnemyDrop()
    {
        
        itemCollider.enabled = !itemCollider.enabled;

        //THROW ANIMATION
        
        itemStatus = 2;
        smoothSpeed = 0.1f;
        lastMoveX = opi.GetComponent<PlayerController>().lastMoveX * 3;
        lastMoveY = opi.GetComponent<PlayerController>().lastMoveY * 3;
        toss = itemPosition + new Vector3(lastMoveX, lastMoveY - 3, 0f);

        //set enemy animation to "not holding"
        itemAnim.SetBool("Item Drop", true);

        StartCoroutine("dropped");

        //SHADOW
        shadow.SetActive(true);
        
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
        opiAnim.SetBool("Scene Trigger", true);
        StartCoroutine("destroyObj");
    }

    IEnumerator dropped()
    {
        yield return new WaitForSeconds(0.5f);

        print("recharge");
        itemCollider.enabled = !itemCollider.enabled;
        holder = 0;
    }

    IEnumerator destroyObj()
    {
        yield return new WaitForSeconds(1f);

        Destroy(this.gameObject);
    }



}
    