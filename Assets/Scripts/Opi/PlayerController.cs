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

    public float lastMoveX;
    public float lastMoveY;

    //Roll
    public bool roll;


    //Triggers
    //private bool standingOnTrigger;
    private bool cauldronTrigger;

    GameObject wizardTrigger;
    bool atWizardTrigger;

    //Interact
    public bool sceneTrigger;
    public bool interact;

    // Update is called once per frame
    public void Update()
    {
        //// INPUT ////


        //Movement
        movement.x = Input.GetAxisRaw("Horizontal");
        movement.y = Input.GetAxisRaw("Vertical");

        stopSpeed.x = 0;
        stopSpeed.y = 0;

        //Roll
        roll = Input.GetButtonDown("roll");

        //Item
        interact = Input.GetButtonDown("interact");

        wizardTrigger = GameObject.Find("DirtPatch");
        atWizardTrigger = wizardTrigger.GetComponent<AddIngredient>().atTrigger;

        //// ANIMATION ////


        //Movement
        animator.SetFloat("Horizontal", movement.x);
        animator.SetFloat("Vertical", movement.y);
        animator.SetFloat("Speed", movement.sqrMagnitude);

        //Roll
        animator.SetBool("Roll", roll);

        //Last Move
        if (Input.GetAxisRaw("Horizontal") > 0.01 || Input.GetAxisRaw("Horizontal") < -0.01 || Input.GetAxisRaw("Vertical") > 0.01 || Input.GetAxisRaw("Vertical") < -0.01)
        {
            animator.SetFloat("Last Move Horizontal", movement.x + movement.x);
            animator.SetFloat("Last Move Vertical", movement.y + movement.y);

            lastMoveX = movement.x + movement.x;
            lastMoveY = movement.y + movement.y;
        }
    }

  

    public void FixedUpdate()
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

        

    }

    public void AddIngredient()
    {
        print("It work");
        animator.SetBool("Scene Trigger", true);
    }


}


