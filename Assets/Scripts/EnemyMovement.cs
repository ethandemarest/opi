using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyMovement : MonoBehaviour
{
    GameObject opi;
    public GameObject slash;
    GameObject damage;

    BoxCollider2D boxCollider;

    //Health
    public HealthBar healthBar;
    public float maxHealth = 10;
    public float currentHealth;

    Rigidbody2D rb;
    Animator animator;
    public float movementSpeed = 0.1f;
    public float knockBackPower = 3;
    public float knockDownTime = 0.5f;
    public float detectRange;
    public float attackRange;
    public float attackSpeed;
    public float attackRecharge;

    public Vector2 movement;
    Vector2 knockBack;
    Vector3 attackMovement;

    bool alive;
    int focus;
    public int state;
    public bool hit;
    bool wasHit;
    bool isAttacking;
    bool attack;
    bool parried;
    float angle;
    


    // Update is called once per frame
    void Start()
    {
        alive = true;
        currentHealth = maxHealth;

        opi = GameObject.Find("Opi");

        damage = this.transform.GetChild(0).gameObject;
        damage.SetActive(false);

        boxCollider = this.GetComponent<BoxCollider2D>();
        rb = this.GetComponent<Rigidbody2D>();
        animator = this.GetComponent<Animator>();
    }

    public void FixedUpdate()
    {
        movement = transform.position - opi.transform.position;

        //BEHAVIOR LIST
        Vector2[] targetPosition = new Vector2[2];
        targetPosition[0] = transform.position; //Enemy
        targetPosition[1] = opi.transform.position; //Opi

        //SPEED LIST
        float[] targetSpeed = new float[2];
        targetSpeed[0] = 0f; //Enemy
        targetSpeed[1] = 1f; //Opi

        //Movement
        animator.SetFloat("Horizontal", -movement.x);
        animator.SetFloat("Vertical", -movement.y);
        animator.SetFloat("Speed", targetSpeed[focus]);

        if (movement.x > 0.1 || movement.x < -0.1 || movement.y > 0.1 || movement.y < -0.1){
            animator.SetFloat("Last Move Horizontal", -(movement.x + movement.x));
            animator.SetFloat("Last Move Vertical", -(movement.y + movement.y));
        }

        //DISTANCE BETWEEN ENEMY & OPI
        float opiDistance = Vector3.Distance(opi.transform.position, transform.position);

        //Movement Expression
        if (hit == true){
            transform.position = Vector2.Lerp(transform.position, knockBack, 0.05f);
        }
        if (attack == true && hit == false){
            transform.position = Vector2.Lerp(transform.position, attackMovement, attackSpeed);
        }
        else{
            transform.position = Vector2.MoveTowards(transform.position, targetPosition[focus], movementSpeed);
        }

        // OPI DETECTION
        if (opiDistance <= detectRange && hit == false && alive == true) // CHASE
        {
            if (opiDistance <= attackRange && isAttacking == false) // ATTACK
            {
                focus = 0;
                StartCoroutine("Attack");
            }
            else if (isAttacking == false)
            {
                focus = 1;
            }
        }
        else // IDLE
        {
            focus = 0;
        }

        if (currentHealth == 0)
        {
            print("killed");
            Destroy(this.gameObject);
        }
    }

    void OnTriggerEnter2D(Collider2D other)
    {

        if (other.CompareTag("Attack") && wasHit == false)
        {
            SendMessage("Hit");
        }

        if (other.CompareTag("Arrow"))
        {
            alive = false;
            animator.SetBool("ArrowHit", true);
            focus = 0;
        }
    }

    void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Attack"))
        {

        }
    }

    public void Hit()
    {
        print("enemy hit");
        TakeDamage(2);

        Vector2 difference = (transform.position - opi.transform.position);
        knockBack.x = transform.position.x + difference.normalized.x * knockBackPower;
        knockBack.y = transform.position.y + difference.normalized.y * knockBackPower;

        animator.SetBool("Hit", true);
        StartCoroutine("HitDelay");
    }

    public void TakeDamage(float damage)
    {
        currentHealth -= damage;
        healthBar.SetHealth(currentHealth);
    }

    IEnumerator HitDelay()
    {
        focus = 0; // Stop
        hit = true;
        wasHit = true;

        yield return new WaitForSeconds(0.2f);

        wasHit = false;


        if (parried == true)
        {
            print("succesful parry");
            yield return new WaitForSeconds(knockDownTime * 2);
        }
        else
        {
            yield return new WaitForSeconds(knockDownTime);
        }

        focus = 1; // Opi

        hit = false;
        parried = false;
    }

    IEnumerator Attack()
    {
        // CHARGE UP
        isAttacking = true;
        animator.SetBool("Attack", true);        
        
        yield return new WaitForSeconds(0.6f);
        //ATTACK

        if (hit == true)
        {
            print("break1");
            isAttacking = false;
            attack = false;
            yield break;
        }

        attack = true;
        attackMovement = opi.transform.position;

        // slash spawn
        Vector3 difference = opi.transform.position - transform.position;
        Vector2 slashPos;
        slashPos.x = transform.position.x + difference.x;
        slashPos.y = transform.position.y + difference.y + 1;
        float sign = (opi.transform.position.y < transform.position.y) ? -1.0f : 1.0f;
        angle = Vector2.Angle(Vector2.right, difference) * sign;
        Instantiate(slash, transform.position + new Vector3(0f, 1.5f), Quaternion.Euler(transform.position.x, transform.position.y, angle));

        
        yield return new WaitForSeconds(0.1f); //PARRY DELAY

        if (hit == true)
        {
            print("break2");
            parried = true;
            isAttacking = false;
            attack = false;
            yield break;
        }
        else
        {
            damage.SetActive(true);
        }

        boxCollider.enabled = !boxCollider.enabled;

        yield return new WaitForSeconds(0.2f);

        //END ATTACK
        attack = false;

        damage.SetActive(false);
        boxCollider.enabled = !boxCollider.enabled;


        yield return new WaitForSeconds(attackRecharge);
        //ATTACK RECHARGE

        attack = false;
        isAttacking = false;
        focus = 1; // Opi
    }    
}
 