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

    Vector2 difference;
    public Vector2 movement;

    public float lastMoveX;
    public float lastMoveY;

    bool hit = false;

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
        Vector2 difference;
        difference.x = transform.position.x - opi.transform.position.x;
        difference.y = transform.position.y - opi.transform.position.y;

        difference = difference.normalized;

        

        transform.position = Vector2.Lerp(transform.position, difference.normalized, 0.05f);

   
        //StartCoroutine("HitDelay");
    }

    IEnumerator HitDelay()
    {
        


        yield return new WaitForSeconds(1f);


    }


    public void FixedUpdate()
    {

        if(hit == false)
        {
            //rb.MovePosition(Vector2.MoveTowards(transform.position, opi.transform.position, speed * Time.deltaTime));
        }
        if(hit == true)
        {

        }

    }

}
