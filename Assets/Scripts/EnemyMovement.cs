using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyMovement : MonoBehaviour
{
    GameObject opi;
    public GameObject damage;
    public GameObject opiCenter;
    public GameObject EnemCenter;

    BoxCollider2D boxCollider;

    //Health
    /*
    public HealthBar healthBar;
    public float maxHealth = 10;
    public float currentHealth;
    */

    Rigidbody2D rb;
    Animator animator;
    public float movementSpeed = 0.1f;
    public float knockBackPower = 3;
    public float knockDownTime = 0.5f;
    public float detectRange;
    public float attackRange;
    public float attackSpeed;
    public float attackRecharge;

    public Vector2 lookDirection;
    Vector2 knockBack;
    Vector3 attackMovement;

    bool alive;
    int focus;
    int eyeFocus;
    public int state;
    public bool hit;
    bool isAttacking;
    public bool attack;
    bool inAir;

    //Time Delay
    int frame;
    public int SlowMoDuration = 60;


    // Update is called once per frame
    void Start()
    {
        eyeFocus = 0;
        attack = false;
        inAir = false;
        alive = true;
        //currentHealth = maxHealth;

        opi = GameObject.Find("Opi");


        boxCollider = this.GetComponent<BoxCollider2D>();
        rb = this.GetComponent<Rigidbody2D>();
        animator = this.GetComponent<Animator>();
    }

    public void FixedUpdate()
    {
        frame++;

        if (frame >= SlowMoDuration)
        {
            Time.timeScale = 1.0f;
        }

        
        Vector2[] lookDir = new Vector2[2];
        lookDir[0] = -(transform.position - opi.transform.position);
        lookDir[1] = -(transform.position - attackMovement);
        this.lookDirection = lookDir[eyeFocus];


        //BEHAVIOR LIST
        Vector2[] targetPosition = new Vector2[2];
        targetPosition[0] = transform.position; //Enemy
        targetPosition[1] = opi.transform.position; //Opi

        //SPEED LIST
        float[] targetSpeed = new float[2];
        targetSpeed[0] = 0f; //Enemy
        targetSpeed[1] = 1f; //Opi

        //Movement
        animator.SetFloat("Horizontal", lookDirection.x);
        animator.SetFloat("Vertical", lookDirection.y);
        animator.SetFloat("Speed", targetSpeed[focus]);

        if (lookDirection.x > 0.1 || lookDirection.x < -0.1 || lookDirection.y > 0.1 || lookDirection.y < -0.1){
            animator.SetFloat("Last Move Horizontal", lookDirection.x + lookDirection.x);
            animator.SetFloat("Last Move Vertical", lookDirection.y + lookDirection.y);
        }

        //DISTANCE BETWEEN ENEMY & OPI
        float opiDistance = Vector3.Distance(opi.transform.position, transform.position);

        //Movement Expression
        if (hit == true)
        {
            transform.position = Vector2.Lerp(this.transform.position, knockBack, 0.1f);
        }
        if (attack == true && hit == false)
        {
            transform.position = Vector2.MoveTowards(transform.position, attackMovement, attackSpeed);
        }
        else
        {
            transform.position = Vector2.MoveTowards(this.transform.position, targetPosition[focus], movementSpeed);
        }

        // OPI DETECTION
        if (opiDistance <= detectRange && hit == false && alive == true) // CHASE
        {
            //ATTACK
            if (opiDistance <= attackRange && isAttacking == false){
                focus = 0;
                StartCoroutine("Attack");
            }
            else if (isAttacking == false){
                focus = 1;
            }
        }
        else // IDLE
        {
            focus = 0;
        }
    }

    void OnTriggerEnter2D(Collider2D other)
    {

        if (other.CompareTag("Attack") && hit == false)
        {
            if (inAir == true)
            {
                ///Instantiate(damage, this.transform.position, Quaternion.Euler(0f, 0f, 0f));
            }

            StopAllCoroutines();
            SendMessage("Hit");
            StartCoroutine("HitDelay");

            isAttacking = false;
            animator.SetBool("Hit", true);
            attack = false;
        }

        if (other.CompareTag("Arrow"))
        {
            if(inAir == true)
            {
                StopAllCoroutines();

                attackMovement = this.transform.position;
                isAttacking = false;
                alive = false;
                attack = false;
                animator.SetBool("ArrowHit Air", true);
                focus = 0;
            }
            else
            {
                StopAllCoroutines();

                attackMovement = this.transform.position;
                isAttacking = false;
                alive = false;
                attack = false;
                animator.SetBool("ArrowHit Ground", true);
                focus = 0;
            }
        }
    }

    public void Hit()
    {
        focus = 0; // Stop
        hit = true;

        Vector2 difference = (EnemCenter.transform.position - opiCenter.transform.position);
        knockBack.x = (this.transform.position.x + difference.normalized.x * knockBackPower);
        knockBack.y = (this.transform.position.y + difference.normalized.y * knockBackPower);
    }

    public void FreezeFrame()
    {
        frame = 0;
        Time.timeScale = 0.15f;
    }

    IEnumerator HitDelay()
    {
        yield return new WaitForSeconds(0.1f);


        yield return new WaitForSeconds(knockDownTime);

        inAir = false;
        hit = false;
        focus = 1; // Opi
        
    }

    IEnumerator Attack()
    {
        // CHARGE UP
        isAttacking = true;
        animator.SetBool("Attack", true);

        yield return new WaitForSeconds(0.6f);
        //ATTACK

        attackMovement = opi.transform.position;
        eyeFocus = 1;
        attack = true;
        inAir = true;

        yield return new WaitForSeconds(0.5f);
        //END ATTACK

        Instantiate(damage, this.transform.position, Quaternion.Euler(0f, 0f, 0f));
        attackMovement = this.transform.position;
        eyeFocus = 0;

        inAir = false;
        attack = false;

        yield return new WaitForSeconds(attackRecharge);
        //ATTACK RECHARGE

        isAttacking = false;
        focus = 1; // Opi
        eyeFocus = 0;

    }
}
 