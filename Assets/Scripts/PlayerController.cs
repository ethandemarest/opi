using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    //Movement
    public Rigidbody2D rb;
    public Animator animator;
    public float moveSpeed = 8f;
    public float diagSpeed = 6f;
    public float rollBoost = 4f;
    public Vector2 movement;
    private Vector2 stopSpeed;
    

    //Roll
    private bool rollTrigger;

    //Triggers
    public bool standingOnTrigger;
    private bool sceneTrigger;
    private bool itemHold;
    public bool itemDrop;

    

    // Update is called once per frame
    void Update()
    {
        //// INPUT ////


        //Movement
        movement.x = Input.GetAxisRaw("Horizontal");
        movement.y = Input.GetAxisRaw("Vertical");

        stopSpeed.x = 0;
        stopSpeed.y = 0;


        //Roll
        if (Input.GetButtonDown("roll"))
        {
            rollTrigger = true;
        }
        else if (Input.GetButtonUp("roll"))
        {
            rollTrigger = false;
        }

        //Item
        if (Input.GetButtonDown("item"))
        {
            itemHold = true;
            itemDrop = false;
        }
        else if (Input.GetButtonUp("item"))
        {
            itemHold = false;
            itemDrop = true;
        }
   


        //// ANIMATION ////

        
        //Movement
        animator.SetFloat("Horizontal", movement.x);
        animator.SetFloat("Vertical", movement.y);
        animator.SetFloat("Speed", movement.sqrMagnitude);

        //Roll
        animator.SetBool("Roll", rollTrigger);

        //Item
        animator.SetBool("Item", itemHold);

        //Last Move
        if (Input.GetAxisRaw("Horizontal") > 0.01 || Input.GetAxisRaw("Horizontal") < -0.01 || Input.GetAxisRaw("Vertical") > 0.01 || Input.GetAxisRaw("Vertical") < -0.01)
        {
            animator.SetFloat("Last Move Horizontal", movement.x + movement.x);
            animator.SetFloat("Last Move Vertical", movement.y + movement.y);
        }
    }

    private void FixedUpdate()
    {

        //// MOVEMENT ////

        //Diagonal Speed Adjustment
        if ((movement.x == 1) && (movement.y == 1))
        {
            moveSpeed = diagSpeed;
        }
        else if ((movement.x == -1) && (movement.y == 1))
        {
            moveSpeed = diagSpeed;
        }
        else if ((movement.x == -1) && (movement.y == -1))
        {
            moveSpeed = diagSpeed;
        }
        else if ((movement.x == 1) && (movement.y == -1))
        {
            moveSpeed = diagSpeed;
        }
        else
        {
            moveSpeed = 8;
        }

        //Roll Boost
        if (this.animator.GetCurrentAnimatorStateInfo(0).IsName("Roll"))
        {
            moveSpeed *= rollBoost;
        }

        // Attack Cancel Move
        if (this.animator.GetCurrentAnimatorStateInfo(0).IsName("Attack 1") || this.animator.GetCurrentAnimatorStateInfo(0).IsName("Attack 2"))
        {
            movement = stopSpeed;
        }
        if (this.animator.GetCurrentAnimatorStateInfo(0).IsName("Attack 1") || this.animator.GetCurrentAnimatorStateInfo(0).IsName("ItemDrop"))
        {
            movement = stopSpeed;
        }

        //Movement Expression
        rb.MovePosition(rb.position + movement * moveSpeed * Time.fixedDeltaTime);


        //Item Drop
        if (sceneTrigger == true && itemDrop == true)
        {
            animator.SetBool("At Cauldron", sceneTrigger);
            animator.SetBool("OpiItemPlace", itemDrop);
        }
        else
        {

            animator.SetBool("OpiItemPlace", false);
        }

    }

    //// TRIGGERS ////

    //Enter Trigger
    public void OnTriggerEnter2D(Collider2D collision)
    {
        standingOnTrigger = true;
        if (collision.gameObject.name == "DirtPatch")
        {
            sceneTrigger = true;
            animator.SetBool("At Cauldron", true);     
        }
    }

    //Exit Trigger
    public void OnTriggerExit2D(Collider2D collision)
    {
        standingOnTrigger = false;
        if (collision.gameObject.name == "DirtPatch")
        {
            sceneTrigger = false;
            animator.SetBool("At Cauldron", false);
        }
    }


    

}


