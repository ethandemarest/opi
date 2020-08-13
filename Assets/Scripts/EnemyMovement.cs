using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyMovement : MonoBehaviour
{
    //Movement
    GameObject opi;
    GameObject enemyCenter;

    Rigidbody2D rb;
    Animator animator;
    float speed = 0.1f;
    public float movementSpeed = 0.1f;
    public float knockBackPower = 3;
    public float knockDownTime = 1f;

    public Vector2 movement;
    Vector2 knockBack;

    float lastMoveX;
    float lastMoveY;
    int behavior = 1;
    bool hit = false;


    // Update is called once per frame
    void Start()
    {
        opi = GameObject.Find("Opi");

        enemyCenter = GameObject.Find("Enemy Center");
        rb = this.GetComponent<Rigidbody2D>();
        animator = this.GetComponent<Animator>();

    }

    public void Update()
    {
        //// ANIMATION ////

        movement = transform.position - opi.transform.position;

        //Movement
        animator.SetFloat("Horizontal", -movement.x);
        animator.SetFloat("Vertical", -movement.y);
        animator.SetFloat("Speed", movement.sqrMagnitude);


        //Last Move
        if (Input.GetAxisRaw("Horizontal") > 0.1 || Input.GetAxisRaw("Horizontal") < -0.1 || Input.GetAxisRaw("Vertical") > 0.1 || Input.GetAxisRaw("Vertical") < -0.1)
        {
            animator.SetFloat("Last Move Horizontal", movement.x + movement.x);
            animator.SetFloat("Last Move Vertical", movement.y + movement.y);

            lastMoveX = movement.x + movement.x;
            lastMoveY = movement.y + movement.y;
        }
    }   

    public void Hit()
    {
        Vector2 difference = (transform.position - opi.transform.position);
        knockBack.x = transform.position.x + difference.normalized.x * knockBackPower;
        knockBack.y = transform.position.y + difference.normalized.y * knockBackPower;

        animator.SetBool("Hit", true);
        StartCoroutine("HitDelay");
    }

    IEnumerator HitDelay()
    {
        behavior = 0;
        rb.bodyType = RigidbodyType2D.Kinematic;

        yield return new WaitForSeconds(knockDownTime);

        behavior = 1;
        rb.bodyType = RigidbodyType2D.Dynamic;
    }

    public void FixedUpdate()
    {
    
        if(behavior == 0)
        {
            print("HIT");
            transform.position = Vector2.Lerp(transform.position, knockBack, 0.05f);
        }
        

        if(behavior == 1)
        {
            print("CHASE");
            rb.MovePosition(Vector2.MoveTowards(transform.position, opi.transform.position, movementSpeed));
        }
    }
}
 