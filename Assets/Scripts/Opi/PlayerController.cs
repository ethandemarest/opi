using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public GameObject currentObject = null;
    Vector3 knockBack;
    public float knockBackPower;

    //Movement
    Rigidbody2D rb;
    Animator animator;
    public float speed = 10f;
    public float rollDelay = 1f;
    public float inputX;
    public float inputY;

    public Vector2 movement;
    private Vector2 stopSpeed;
    public Vector2 lastMove;
    private Vector2 rollAngle;
    private Vector2 rollDirection;

    public float lastMoveX;
    public float lastMoveY;

    //Roll
    int inputSource;
    bool rolling;
    bool roll;
    bool canRoll;
    public float rollBoost;
    private Vector2 velocity = Vector2.zero;

    //Interact
    public bool sceneTrigger;
    public bool interact;
    bool hit;
    bool wasHit;

    // Update is called once per frame
    void Start()
    {
        rb = this.GetComponent<Rigidbody2D>();
        animator = this.GetComponent<Animator>();
        rolling = false;
        canRoll = true;
        wasHit = false;
        inputSource = 0;
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

        //Item
        interact = Input.GetButtonDown("interact");

        //// ANIMATION ////

        Vector3[] input = new Vector3[2];
        input[0] = new Vector3(movement.x, movement.y, 0f);
        input[1] = new Vector3(rollDirection.x, rollDirection.y, 0f);

        //Movement

        animator.SetFloat("Horizontal", input[inputSource].x);
        animator.SetFloat("Vertical", input[inputSource].y);
        animator.SetFloat("Speed", movement.sqrMagnitude);

        //Last Move
        if (Input.GetAxisRaw("Horizontal") > 0.1 || Input.GetAxisRaw("Horizontal") < -0.1 || Input.GetAxisRaw("Vertical") > 0.1 || Input.GetAxisRaw("Vertical") < -0.1)
        {
            lastMoveX = movement.x + movement.x;
            lastMoveY = movement.y + movement.y;
            lastMove.x = movement.x;
            lastMove.y = movement.y;

            animator.SetFloat("Last Move Horizontal", lastMoveX);
            animator.SetFloat("Last Move Vertical", lastMoveY);
        }
    }// ANIMATION

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Damage"))
        {
            currentObject = other.gameObject;
            StartCoroutine("Hit");
        }
        else
        {
            currentObject = null;
        }
    }
    
    public void AddIngredient()
    {
        animator.SetBool("Scene Trigger", true);
    }

    IEnumerator rollRecharge()
    {
        rollAngle = transform.position;
        rollDirection = lastMove.normalized;
        rolling = true;
        inputSource = 1;

        // I DONT KNOW HOW THIS WORKS YET
        //rb.AddForce(rollDirection * rollBoost);

        yield return new WaitForSeconds(rollDelay);

        rolling = false;
        canRoll = true;
        inputSource = 0;
    }

    IEnumerator Hit()
    {
        Vector3 difference = (transform.position - currentObject.GetComponent<Transform>().position);
        animator.SetBool("Hit", true);

        knockBack.x = transform.position.x + difference.normalized.x * knockBackPower;
        knockBack.y = transform.position.y + difference.normalized.y * knockBackPower;

        wasHit = true;
        canRoll = false;
        print("hit");

        yield return new WaitForSeconds(0.5f);

        wasHit = false;
        canRoll = true;
        print("safe");
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

        if (rolling == true)
        {
            //rb.MovePosition(Vector2.Lerp(transform.position, rollAngle + rollDirection * rollBoost, 0.1f));
            rb.MovePosition(rb.position +  rollDirection * rollBoost * Time.fixedDeltaTime);
        }

        if (wasHit == true)
        {
            rb.MovePosition(Vector2.Lerp(transform.position, knockBack, 0.05f));
        }
        else if (rolling == false)
        {
            rb.MovePosition(rb.position + movement.normalized * speed * Time.fixedDeltaTime);
        }
            
        
    }
}


