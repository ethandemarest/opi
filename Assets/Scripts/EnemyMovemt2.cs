using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using EZCameraShake;

public class EnemyMovemt2 : MonoBehaviour
{
    GameObject opi;
    GameObject opiCenter;
    GameObject enemCenter;
    GameObject enemyDetect;
    public GameObject nearbyEnemy = null;
    public GameObject deathAnimation;

    Animator animator;

    Vector2 attackTarget;
    Vector2 otherEnemyPosition;
    public Vector2 enemDifference;
    Vector2 surroundBack;
    Vector2 surroundSide;

    PlayerController pc;

    public float currentHealth;

    public SpriteRenderer sprite;

    public float opiDetectRange;
    public float minDistance = 5.0f;
    public float movementSpeed = 100f;
    public float acceleration = 1f;
    private float curSpeed = 0.0f;

    public float knockBackPower = 3;
    public float knockDownTime = 0.6f;

    Vector2 lookDirection;

    Vector2 knockBack;
    Vector2 opiLastMove;
    Vector3 offset;

    int focus;
    int eyeFocus;

    bool knockBackMovement;
    bool attackMovement;
    public bool canAttack;


    // Update is called once per frame
    void Start()
    {
        animator = GetComponent<Animator>();
        sprite = GetComponent<SpriteRenderer>();
        focus = 3;
        knockBackMovement = false;
        attackMovement = false;
        canAttack = true;

        opi = GameObject.Find("Opi");
        pc = GameObject.Find("Opi").GetComponent<PlayerController>();
        opiCenter = GameObject.Find("Opi Center");
        enemCenter = transform.GetChild(0).gameObject;
        offset = transform.position - enemCenter.transform.position;

    }

    public void FixedUpdate()
    {
        //Random.Range(2, 5)
        surroundBack.x = (opi.transform.position.x - pc.lastMove.x * 2);
        surroundBack.y = (opi.transform.position.y - pc.lastMove.y * 2);

        surroundSide.x = opi.transform.position.x + pc.lastMove.y * 2;
        surroundSide.y = opi.transform.position.y + pc.lastMove.x * 2;


        //ANIMATION

        /*
        Vector2[] lookDir = new Vector2[2];
        lookDir[0] = -(transform.position - opi.transform.position);
        lookDirection = lookDir[eyeFocus];
        */


        animator.SetFloat("Horizontal", lookDirection.x);
        animator.SetFloat("Vertical", lookDirection.y);
        
        //BEHAVIOR LIST
        Vector2[] targetPosition = new Vector2[4];
        targetPosition[0] = transform.position; //Enemy
        targetPosition[1] = opiCenter.transform.position + offset; //Opi
        targetPosition[2] = surroundBack;
        targetPosition[3] = surroundSide;

        //SPEED LIST
        float[] targetSpeed = new float[2];
        targetSpeed[0] = 0f; //Enemy
        targetSpeed[1] = 1f; //Opi

        //Movement


        //DISTANCE BETWEEN ENEMY & OPI
        float opiDistance = Vector2.Distance(opiCenter.transform.position, (transform.position));

        if (knockBackMovement == true) 
        {
            // KNOCK BACK MOVEMENT
            if (curSpeed > movementSpeed)
                curSpeed = movementSpeed;
            curSpeed -= acceleration*0.3f;
            transform.position = Vector2.MoveTowards(transform.position, knockBack, Mathf.Clamp(curSpeed,0f,500f)/500);
        }
        else if (attackMovement == true) 
        {
            //ATTACK MOVEMENT
            transform.position = Vector2.Lerp(transform.position, attackTarget, 0.2f);
        }
        else if(opiDistance <= opiDetectRange && canAttack == true) 
        {
            // CAN I SEE OPI?
            if (curSpeed > movementSpeed)
                curSpeed = movementSpeed;

            lookDirection.x = -(transform.position.x - targetPosition[focus].x);
            lookDirection.y = -(transform.position.y - targetPosition[focus].y);


            animator.SetFloat("Speed", curSpeed);
            curSpeed += acceleration;
            transform.position = Vector2.MoveTowards(transform.position, targetPosition[focus], curSpeed / 1000);

        }
        else // OPI IS NOT RANGE
        {
            animator.SetFloat("Speed", 0f);
            curSpeed = 0f;
        }
        if (opiDistance <= minDistance && canAttack) //OPI CLOSE ENOUGH TO ATTACK?
        {
            StartCoroutine("Attack");
            curSpeed = 0f;
        }
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("OpiDamage"))
        {
            //TakeDamage(2);
            StopAllCoroutines();
            StartCoroutine("SwordHit");
            FindObjectOfType<AudioManager>().Play("Sword Hit");
            FindObjectOfType<AudioManager>().Play("Enemy Hurt");
        }

        if (other.CompareTag("Arrow"))
        {
            StopAllCoroutines();
            StartCoroutine("ArrowHit");
            FindObjectOfType<AudioManager>().Play("Arrow Impact");
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if(collision.gameObject.tag == "Opi")
        {
            StartCoroutine("PostAttackDelay");
        }
    }

    /*
    public void TakeDamage(float damage)
    {
        currentHealth -= damage;

        if(currentHealth <= 0)
        {
            Instantiate(deathAnimation, transform.position, Quaternion.identity);
            Destroy(gameObject);
        }
    }
    */

    IEnumerator Attack()
    {
        focus = 0;
        curSpeed = 0;
        attackTarget = opi.transform.position;
        lookDirection = -(transform.position - opi.transform.position);

        canAttack = false;
        sprite.color = new Color(1, 1, 1, 0.5f);

        yield return new WaitForSeconds(0.4f);

        attackMovement = true;
        sprite.color = new Color(1, 1, 1, 1f);

        yield return new WaitForSeconds(0.5f);

        attackMovement = false;

        yield return new WaitForSeconds(1f);

        focus = Random.Range(1,4);
        curSpeed = 0;
        canAttack = true;
    }

    IEnumerator SwordHit()
    {
        SendMessage("DamageEffect");
        CameraShaker.Instance.ShakeOnce(2f, 2f, .1f, 1f);
        focus = 0; // Stop
        opiLastMove.x = pc.lastMove.x;
        opiLastMove.y = pc.lastMove.y;
        knockBack.x = (transform.position.x + opiLastMove.x * knockBackPower);
        knockBack.y = (transform.position.y + opiLastMove.y * knockBackPower);

        sprite.color = new Color(1, 1, 1, 0.5f);

        yield return new WaitForSeconds(0.15f);

        animator.SetBool("Hit", true);
        sprite.color = new Color(1, 1, 1, 1);

        curSpeed = movementSpeed * 2;
        knockBackMovement = true;

        yield return new WaitForSeconds(knockDownTime);

        animator.SetBool("Hit", false);

        knockBackMovement = false;
        canAttack = true;
        curSpeed = 0f;
        focus = Random.Range(1, 4);
    }

    IEnumerator ArrowHit()
    {
        focus = 0; // Stop
        animator.SetBool("ArrowHit Ground", true);

        yield return new WaitForSeconds(knockDownTime);
    }


    IEnumerator PostAttackDelay()
    {
        focus = 0; // Stop
        curSpeed = movementSpeed;

        Vector2 difference = (enemCenter.transform.position - opiCenter.transform.position);

        knockBack.x = (enemCenter.transform.position.x + difference.normalized.x * (knockBackPower / 4));
        knockBack.y = (enemCenter.transform.position.y + difference.normalized.y * (knockBackPower / 4));

        knockBackMovement = true;

        yield return new WaitForSeconds(knockDownTime);
        knockBackMovement = false;
        curSpeed = 0f;
        focus = Random.Range(1, 4);
    }

}
