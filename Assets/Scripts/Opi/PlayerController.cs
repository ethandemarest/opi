using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    //Movement
    Rigidbody2D rb;
    Animator animator;
    public float speed = 10f;
    public float rollDelay = 1f;
    public float inputX;
    public float inputY;

    public Vector2 movement;
    private Vector2 stopSpeed;

    public float lastMoveX;
    public float lastMoveY;

    //Roll
    bool roll;
    bool canRoll;
    float rollBoost = 0f;

    //Interact
    public bool sceneTrigger;
    public bool interact;

    // Update is called once per frame
    void Start()
    {
        rb = this.GetComponent<Rigidbody2D>();
        animator = this.GetComponent<Animator>();
        canRoll = true;
    }

    public void Update()
    {
        //// INPUT ////

        //Movement

        inputX = Input.GetAxisRaw("Horizontal");
        inputY = Input.GetAxisRaw("Vertical");

        movement.x = inputX;
        movement.y = inputY;

        stopSpeed.x = 0;
        stopSpeed.y = 0;

        //Roll
        roll = Input.GetButtonDown("roll");

        if (roll && canRoll == true)
        {
            canRoll = false;
            animator.SetBool("Roll", roll);
            StartCoroutine("rollRecharge");
        }

        if (this.animator.GetCurrentAnimatorStateInfo(0).IsName("Roll"))
        {
            rollBoost = 5f;
        }
        else
        {
            rollBoost = 0f;
        }

        //Item
        interact = Input.GetButtonDown("interact");

        //// ANIMATION ////

        //Movement
        animator.SetFloat("Horizontal", movement.x);
        animator.SetFloat("Vertical", movement.y);
        animator.SetFloat("Speed", movement.sqrMagnitude);

        //Last Move
        if (Input.GetAxisRaw("Horizontal") > 0.1 || Input.GetAxisRaw("Horizontal") < -0.1 || Input.GetAxisRaw("Vertical") > 0.1 || Input.GetAxisRaw("Vertical") < -0.1)
        {
            lastMoveX = movement.x + movement.x;
            lastMoveY = movement.y + movement.y;

            animator.SetFloat("Last Move Horizontal", lastMoveX);
            animator.SetFloat("Last Move Vertical", lastMoveY);
        }
    }

  

    public void FixedUpdate()
    {

        //// MOVEMENT ////

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

        //Vector2.ClampMagnitude(movement, 1)
        rb.MovePosition(rb.position + movement.normalized * (speed + rollBoost) * Time.fixedDeltaTime);     
    }

    public void AddIngredient()
    {
        animator.SetBool("Scene Trigger", true);
    }

    IEnumerator rollRecharge()
    {
        
        yield return new WaitForSeconds(rollDelay);
        
        canRoll = true;
    }
}


