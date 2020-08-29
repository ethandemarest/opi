using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyMovement : MonoBehaviour
{
    //Movement
    GameObject opi;

    Rigidbody2D rb;
    Animator animator;
    public float movementSpeed = 0.1f;
    public float knockBackPower = 3;
    public float knockDownTime = 0.5f;
    public float detectRange = 10f;
    public float attackRange = 2f;

    public Vector2 movement;
    Vector2 knockBack;

    float lastMoveX;
    float lastMoveY;
    int behavior = 0;
    public bool hit = false;


    // Update is called once per frame
    void Start()
    {
        opi = GameObject.Find("Opi");
        rb = this.GetComponent<Rigidbody2D>();
        animator = this.GetComponent<Animator>();
    }

    public void Update()
    {
        //// ANIMATION ////
       
        //Movement
        animator.SetFloat("Horizontal", -movement.x);
        animator.SetFloat("Vertical", -movement.y);
        animator.SetFloat("Speed", movement.sqrMagnitude);

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
        behavior = 1;
        rb.bodyType = RigidbodyType2D.Kinematic;
        hit = true;

        yield return new WaitForSeconds(knockDownTime);

        behavior = 0;
        rb.bodyType = RigidbodyType2D.Dynamic;
        hit = false;
    }

    public void FixedUpdate()
    {

        float opiDistance = Vector3.Distance(opi.transform.position, transform.position);

        //FOLLOW & ATTACK OPI
        if (behavior == 0)
        {
            if(opiDistance <= attackRange)
            {
                rb.MovePosition(Vector2.MoveTowards(transform.position, transform.position, movementSpeed));
                movement = transform.position - transform.position;
                print("attack");
            }
            else if (opiDistance <= detectRange) 
            {
                rb.MovePosition(Vector2.MoveTowards(transform.position, opi.transform.position, movementSpeed));
                movement = transform.position - opi.transform.position;
            }
            else
            {
                rb.MovePosition(Vector2.MoveTowards(transform.position, transform.position, movementSpeed));
                movement = transform.position - transform.position;
            }

            //Last Move
            if (movement.x > 0.1 || movement.x < -0.1 || movement.y > 0.1 || movement.y < -0.1)
            {
                animator.SetFloat("Last Move Horizontal", -(movement.x + movement.x));
                animator.SetFloat("Last Move Vertical", -(movement.y + movement.y));

                lastMoveX = movement.x - movement.x;
                lastMoveY = movement.y - movement.y;
            }
        }

        //HIT
        if (behavior == 1)
        {
            transform.position = Vector2.Lerp(transform.position, knockBack, 0.05f);
        }


    }
}
 