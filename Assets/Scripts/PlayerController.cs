using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public Rigidbody2D rb;
    public Animator animator;

    public float moveSpeed = 8f;
    public float diagSpeed = 6f;
    public float rollBoost = 4f;

    Vector2 movement;




    private bool rollTrigger;
    private bool itemHold;
    public bool itemDrop;

    

    // Update is called once per frame
    void Update()
    {

        //Input

        movement.x = Input.GetAxisRaw("Horizontal");
        movement.y = Input.GetAxisRaw("Vertical");


        //Attack

        
      



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


        //Animation

        
        //Movement
        animator.SetFloat("Horizontal", movement.x);
        animator.SetFloat("Vertical", movement.y);
        animator.SetFloat("Speed", movement.sqrMagnitude);

        //Attack
        //animator.SetBool("Attack", attackOne);
        //animator.SetBool("Attack 2", attackTwo);
        
         //Roll
        animator.SetBool("Roll", rollTrigger);

        //Item
        animator.SetBool("Item", itemHold);
    }

    private void FixedUpdate()  
    {
        //Attack
      






        //Last Move
        if (Input.GetAxisRaw("Horizontal") > 0.01 || Input.GetAxisRaw("Horizontal") < -0.01 || Input.GetAxisRaw("Vertical") > 0.01 || Input.GetAxisRaw("Vertical") < -0.01)
        {
            animator.SetFloat("Last Move Horizontal", movement.x + movement.x);
            animator.SetFloat("Last Move Vertical", movement.y + movement.y);
        }


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
            moveSpeed = moveSpeed * rollBoost;
        }

        //Movement Expression
        rb.MovePosition(rb.position + movement * moveSpeed * Time.fixedDeltaTime);

    }

}
