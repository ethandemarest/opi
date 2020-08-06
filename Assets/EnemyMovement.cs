using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyMovement : MonoBehaviour
{
    //Movement
    GameObject opi;

    Rigidbody2D rb;
    Animator animator;
    public float speed = 3f;
    public float diagSpeed = 2f;
    public Vector3 offset;

    public Vector2 movement;

    public float lastMoveX;
    public float lastMoveY;

    bool run = true;

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
        movement.x = transform.position.x - opi.transform.position.x;
        movement.y = transform.position.y - opi.transform.position.y;

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



    public void FixedUpdate()
    {
        //// MOVEMENT ////



        //Movement Expression
        //rb.MovePosition(rb.position + OperatingSystemFamily. * moveSpeed * Time.fixedDeltaTime);
        if(run == true)
        {
            rb.MovePosition(Vector2.MoveTowards(transform.position, opi.transform.position + offset, speed * Time.deltaTime));
        }

    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Opi"))
        {
            run = false;
        }
    }
    void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Opi"))
        {
            run = true;
        }
    }

}
