using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyMovemt2 : MonoBehaviour
{
    public bool startAlive;

    [Header("||Game Objects||")]
    public GameObject slashEffect;
    public GameObject hitEffect;
    public GameObject damage;
    public GameObject healthbar;
    public GameObject spawner;

    GameObject opi;
    GameObject opiCenter;
    GameObject enemCenter;

    [Header("||Movement||")]
    public float movementSpeed = 100f;
    public float acceleration = 1f;
    float curSpeed = 0.0f;

    CapsuleCollider2D damCollider;
    CapsuleCollider2D enemHitbox;
    Animator animator;
    PlayerController pc;

    [Header("||Combat||")]
    public float opiDetectRange;
    public float attackDistance = 5.0f;
    public float attackRange;
    public float knockBackPower = 3;
    public float knockDownTime = 0.6f;

    Vector2 aim;
    Vector2 attackTarget;
    Vector2 surroundBack;
    Vector2 surroundSide;
    Vector2 lookDirection;
    Vector2 knockBack;
    Vector2 opiLastMove;
    Vector3 offset;

    float angle;
    float opiDistance;
    int focus;
    int behavior;
    bool canMove;
    bool opiAlive;

    // Update is called once per frame
    void Start()
    {
        if (startAlive == false)
        {
            gameObject.SetActive(false);
        }

        animator = GetComponent<Animator>();
        focus = 1;
        canMove = true;

        opi = GameObject.Find("Opi");
        pc = GameObject.Find("Opi").GetComponent<PlayerController>();
        opiCenter = GameObject.Find("Opi Center");
        enemCenter = transform.GetChild(0).gameObject;
        offset = transform.position - enemCenter.transform.position;
        enemHitbox = GetComponent<CapsuleCollider2D>();

        damCollider = damage.GetComponent<CapsuleCollider2D>();
        damCollider.enabled = false;
    }

    public void FixedUpdate()
    {
        //DISTANCE BETWEEN ENEMY & OPI
        opiAlive = opi.GetComponent<PlayerController>().alive;
        opiDistance = Vector2.Distance(opiCenter.transform.position, (transform.position));

        //Random.Range(2, 5)
        surroundBack.x = (opi.transform.position.x - pc.lastMove.x * 2);
        surroundBack.y = (opi.transform.position.y - pc.lastMove.y * 2);

        surroundSide.x = opi.transform.position.x + pc.lastMove.y * 2;
        surroundSide.y = opi.transform.position.y + pc.lastMove.x * 2;

        //Aim Calculation
        aim.x = enemCenter.transform.position.x - opiCenter.transform.position.x;
        aim.y = enemCenter.transform.position.y - opiCenter.transform.position.y;
        spawner.transform.position = transform.position + Vector3.ClampMagnitude(aim, 2) - (transform.position - enemCenter.transform.position);
        angle = Mathf.Atan2(-aim.y, -aim.x) * Mathf.Rad2Deg - 180f;

        //ANIMATION
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
        if (canMove)
        {
            if (opiDistance <= opiDetectRange && opiAlive){
                behavior = 1;
            }
            else{
                behavior = 0;
            }
            if (opiDistance <= attackDistance && opiAlive)
            {
                StartCoroutine("Attack");
            }
        }
       
        if(behavior == 0) //IDLE
        {
            lookDirection.x = -(transform.position.x - targetPosition[focus].x);
            lookDirection.y = -(transform.position.y - targetPosition[focus].y);
            animator.SetFloat("Speed", 0f);
            curSpeed = 0f;
        }
        if(behavior == 1) //FOLLOW OPI
        {
            if (curSpeed > movementSpeed)
                curSpeed = movementSpeed;

            lookDirection.x = -(transform.position.x - targetPosition[focus].x);
            lookDirection.y = -(transform.position.y - targetPosition[focus].y);

            animator.SetFloat("Speed", curSpeed);
            curSpeed += acceleration;
            transform.position = Vector2.MoveTowards(transform.position, targetPosition[focus], curSpeed / 1000);
        }
        if(behavior == 2) //ATTACK OPI
        {
            transform.position = Vector2.Lerp(transform.position, attackTarget, 0.2f);
        }
        if(behavior == 3) //KNOCKBACK
        {
            if (curSpeed > movementSpeed)
                curSpeed = movementSpeed;
            curSpeed -= acceleration * 0.3f;
            transform.position = Vector2.MoveTowards(transform.position, knockBack, Mathf.Clamp(curSpeed, 0f, 500f) / 500);
        }
        if(behavior == 4) //FLEE
        {
            animator.SetFloat("Speed", curSpeed);
            lookDirection.x = transform.position.x-attackTarget.x;
            lookDirection.y = transform.position.y-attackTarget.y;

            if (curSpeed > movementSpeed)
                curSpeed = movementSpeed;
            curSpeed += acceleration;
            transform.position = Vector2.MoveTowards(transform.position, attackTarget, (curSpeed / 1000) * -1);
        }
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("OpiDamage"))
        {
            StopAllCoroutines();
            healthbar.SendMessage("UseEnergy", 1);
            StartCoroutine("SwordHit");
            FindObjectOfType<AudioManager>().Play("Sword Hit");
            FindObjectOfType<AudioManager>().Play("Enemy Hurt");
        }

        if (other.CompareTag("Arrow"))
        {
            healthbar.SendMessage("UseEnergy", 3);
            StopAllCoroutines();
            StartCoroutine("ArrowHit");
            FindObjectOfType<AudioManager>().Play("Arrow Impact");
        }
    }

    IEnumerator Death()
    {
        StopAllCoroutines();
        canMove = false;
        behavior = 0;
        enemHitbox.enabled = false;
        animator.SetBool("Death", true);
        FindObjectOfType<AudioManager>().Play("Arrow Impact");

        yield return new WaitForSeconds(1f);

        Destroy(gameObject);
    }

    IEnumerator Attack()
    {
        behavior = 0;
        curSpeed = 0;
        canMove = false;

        animator.SetBool("Attack", true);

        //BEGIN ATTACK
        yield return new WaitForSeconds(0.2f);

        attackTarget = transform.position - Vector3.ClampMagnitude((transform.position - opi.transform.position), attackRange);
        lookDirection = -(transform.position - opi.transform.position);

        yield return new WaitForSeconds(0.4f);

        behavior = 2;

        slashEffect.GetComponent<EnemySlashes>().SendMessage("SpawnSlash", lookDirection);

        damage.transform.position = transform.position - offset - Vector3.ClampMagnitude(transform.position - opiCenter.transform.position, 1);
        damCollider.enabled = true;

        yield return new WaitForSeconds(1f);

        behavior = 0;
        damCollider.enabled = false;
        StartCoroutine("PostAttackDelay");

    }

    IEnumerator PostAttackDelay()
    {
        yield return new WaitForSeconds(Random.Range(0.5f, 1f));

        curSpeed = 0;
        behavior = 4;
        focus = 1;

        //RETREAT TIME
        yield return new WaitForSeconds(Random.Range(0.5f, 1f));

        behavior = 0;

        //DELAY TIME
        yield return new WaitForSeconds(Random.Range(0f,1.5f));

        canMove = true;
        focus = 1;
        curSpeed = 0;
    }

    IEnumerator SwordHit()
    {
        behavior = 3;
        canMove = false;
        damCollider.enabled = false;
        focus = 0; // Stop
        opiLastMove.x = pc.lastMove.x;
        opiLastMove.y = pc.lastMove.y;
        knockBack.x = (transform.position.x + opiLastMove.x * knockBackPower);
        knockBack.y = (transform.position.y + opiLastMove.y * knockBackPower);

        Instantiate(hitEffect, spawner.transform.position, Quaternion.Euler(0f, 0f, angle));

        animator.SetBool("Hit", true);
        curSpeed = movementSpeed;

        yield return new WaitForSeconds(knockDownTime);

        animator.SetBool("Hit", false);

        behavior = 1;
        curSpeed = 0f;
        focus = Random.Range(1, 4);
        canMove = true;
    }

    IEnumerator ArrowHit()
    {
        focus = 0; // Stop
        animator.SetBool("ArrowHit Ground", true);

        yield return new WaitForSeconds(knockDownTime);
    }
}
