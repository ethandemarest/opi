using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyMovement : MonoBehaviour
{
    //Movement
    GameObject opi;
    public GameObject slash;
    GameObject damage;

    BoxCollider2D boxCollider;
    CircleCollider2D damageCollider;

    Rigidbody2D rb;
    Animator animator;
    public float movementSpeed = 0.1f;
    public float knockBackPower = 3;
    public float knockDownTime = 0.5f;
    public float detectRange = 10f;
    public float attackRange = 2f;
    public float attackSpeed;

    public Vector2 movement;
    Vector2 knockBack;
    Vector3 attackMovement;

    int focus;
    int state;
    public bool hit = false;
    bool isAttacking;
    bool attack;
    float angle;
    


    // Update is called once per frame
    void Start()
    {
        state = 1; // Follow
        opi = GameObject.Find("Opi");

        damage = this.transform.GetChild(0).gameObject;
        damageCollider = damage.GetComponent<CircleCollider2D>();
        damageCollider.enabled = !damageCollider.enabled;


        boxCollider = this.GetComponent<BoxCollider2D>();
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
        focus = 0; // Stop
        state = 2; // Hit
        hit = true;

        yield return new WaitForSeconds(knockDownTime);

        focus = 1; // Opi
        state = 1; // Follow
        hit = false;
    }

    IEnumerator Attack()
    {
        attackMovement = transform.position - (transform.position - opi.transform.position)*2;
        isAttacking = true;
        focus = 0; // Stop
        state = 3; // Attack
        animator.SetBool("Attack", true);
        boxCollider.enabled = !boxCollider.enabled;

        Vector3 difference = opi.transform.position - transform.position;
        Vector2 slashPos;
        slashPos.x = transform.position.x + difference.x;
        slashPos.y = transform.position.y + difference.y + 1;

        float sign = (opi.transform.position.y < transform.position.y) ? -1.0f : 1.0f;
        angle = Vector2.Angle(Vector2.right, difference) * sign;

        yield return new WaitForSeconds(0.6f);

        if (!this.animator.GetCurrentAnimatorStateInfo(0).IsName("Enem_Hit_Front"))
        {
            Instantiate(slash, slashPos, Quaternion.Euler(transform.position.x, transform.position.y, angle));
        }

        attack = true;
        damageCollider.enabled = !damageCollider.enabled;
        StartCoroutine("AttackRechange");
    }

    IEnumerator AttackRechange()
    {
        yield return new WaitForSeconds(0.4f);

        attack = false;
        isAttacking = false;
        focus = 1; // Opi
        state = 1; // Follow

        boxCollider.enabled = boxCollider.enabled;
        damageCollider.enabled = !damageCollider.enabled;


    }

    public void FixedUpdate()
    {
        //DISTANCE BETWEEN ENEMY & OPI
        float opiDistance = Vector3.Distance(opi.transform.position, transform.position);

        //BEHAVIOR LIST
        Vector3[] position = new Vector3[2];
        position[0] = transform.position; //Enemy
        position[1] = opi.transform.position; //Opi

        if (state == 1) //FOLLOW & ATTACK OPI
        {
            if (opiDistance >= attackRange)
            {
                focus = 1; // Opi
            }
            else if (isAttacking == false)
            {
                StartCoroutine("Attack");
            }

            movement = transform.position - opi.transform.position;
            rb.MovePosition(Vector2.MoveTowards(transform.position, position[focus], movementSpeed));

            //Last Move
            if (movement.x > 0.1 || movement.x < -0.1 || movement.y > 0.1 || movement.y < -0.1)
            {
                animator.SetFloat("Last Move Horizontal", -(movement.x + movement.x));
                animator.SetFloat("Last Move Vertical", -(movement.y + movement.y));
            }
        }

        if (state == 2) //HIT
        {
            transform.position = Vector2.Lerp(transform.position, knockBack, 0.05f);
        }

        if (state == 3) //Attack
        {
            if(attack == true)
            {
                transform.position = Vector2.Lerp(transform.position, attackMovement, attackSpeed);
            }
        }


        
    }
}
 