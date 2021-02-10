using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using EZCameraShake;

public class EnemyShooterMovement : MonoBehaviour
{
    GameObject opi;
    GameObject opiCenter;
    GameObject enemCenter;

    public GameObject projectile;
    public GameObject spawner;
    public GameObject damageSpawner;
    public GameObject damage;

    public Vector2 aim;

    public Vector2 enemDifference;
    Vector2 surroundBack;
    Vector2 surroundSide;

    PlayerController pc;

    public float currentHealth;

    public SpriteRenderer sprite;
    Animator animator;

    public float opiDetectRange;    
    public float minDistance = 5.0f;
    public float movementSpeed = 100f;
    public float acceleration = 1f;
    private float curSpeed = 0.0f;

    public float knockBackPower = 3;
    public float knockDownTime = 0.6f;

    float angle;
    float angle2;

    Vector2 lookDirection;

    Vector2 knockBack;
    Vector2 opiLastMove;
    Vector3 offset;

    public int stagger;
    public int staggerBreak;
    public float staggerTime;

    int focus;

    bool knockBackMovement;
    public bool canAttack;


    // Update is called once per frame
    void Start()
    {
        sprite = GetComponent<SpriteRenderer>();
        focus = 1;
        knockBackMovement = false;
        canAttack = true;
        animator = GetComponent<Animator>();

        opi = GameObject.Find("Opi");
        pc = GameObject.Find("Opi").GetComponent<PlayerController>();
        opiCenter = GameObject.Find("Opi Center");
        enemCenter = transform.GetChild(0).gameObject;
        //offset = transform.position - enemCenter.transform.position;

    }

    public void FixedUpdate()
    {

        //Random.Range(2, 5)
        surroundBack.x = (opi.transform.position.x - pc.lastMove.x * 2);
        surroundBack.y = (opi.transform.position.y - pc.lastMove.y * 2);

        surroundSide.x = opi.transform.position.x + pc.lastMove.y * 2;
        surroundSide.y = opi.transform.position.y + pc.lastMove.x * 2;




        //ANIMATION

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


        //Aim Calculation
        aim.x = transform.position.x - opiCenter.transform.position.x;
        aim.y = transform.position.y - opiCenter.transform.position.y;
        spawner.transform.position = transform.position + Vector3.ClampMagnitude(-aim, 2) - (transform.position - enemCenter.transform.position);
        angle = Mathf.Atan2(aim.y, aim.x) * Mathf.Rad2Deg - 180f;

        damageSpawner.transform.position = transform.position + Vector3.ClampMagnitude(aim, 0.5f) + new Vector3 (0f,1f,0f);

        animator.SetFloat("Horizontal", -aim.x);
        animator.SetFloat("Vertical", -aim.y);


        if (knockBackMovement == true) 
        {
            // KNOCK BACK MOVEMENT
            curSpeed -= acceleration*0.3f;
            transform.position = Vector2.MoveTowards(transform.position, knockBack, Mathf.Clamp(curSpeed,0f,500f)/500);
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
            StopAllCoroutines();
            StartCoroutine("SwordHit");

            FindObjectOfType<AudioManager>().Play("Sword Hit");
            FindObjectOfType<AudioManager>().Play("Enemy Hurt");


            stagger++;
            if(stagger >= staggerBreak)
            {
                //TakeDamage(2);
            }
        }

        if (other.CompareTag("Arrow"))
        {
            Destroy(other.gameObject);
            StopAllCoroutines();
            StartCoroutine("SwordHit");

            FindObjectOfType<AudioManager>().Play("Sword Hit");
            FindObjectOfType<AudioManager>().Play("Enemy Hurt");
            FindObjectOfType<AudioManager>().Play("Spellcaster Deflect");
        }
    }

    IEnumerator Stagger()
    {
        yield return new WaitForSeconds(staggerTime);
        stagger = 0;
    }

    IEnumerator Attack()
    {
        focus = 0;
        curSpeed = 0;
        lookDirection = -(transform.position - opi.transform.position);
        animator.SetBool("Attack", true);

        canAttack = false;

        yield return new WaitForSeconds(0.9f);
        Instantiate(projectile, spawner.transform.position, Quaternion.Euler(0f,0f,angle));
        yield return new WaitForSeconds(0.5f);
        Instantiate(projectile, spawner.transform.position, Quaternion.Euler(0f, 0f, angle));
        yield return new WaitForSeconds(0.5f);
        Instantiate(projectile, spawner.transform.position, Quaternion.Euler(0f, 0f, angle));
        yield return new WaitForSeconds(1f);

        focus = Random.Range(1,4);
        curSpeed = 0;
        canAttack = true;
    }

    IEnumerator SwordHit()
    {
        animator.SetBool("Hit", true);
        CameraShaker.Instance.ShakeOnce(2f, 2f, .1f, 1f);

        focus = 0; // Stop
        curSpeed = 200;
        opiLastMove.x = pc.lastMove.x;
        opiLastMove.y = pc.lastMove.y;
        knockBack.x = (transform.position.x + opiLastMove.x * knockBackPower);
        knockBack.y = (transform.position.y + opiLastMove.y * knockBackPower);
        SendMessage("DamageEffect");


        sprite.color = new Color(1, 1, 1, 0.5f);

        yield return new WaitForSeconds(0.15f);

        sprite.color = new Color(1, 1, 1, 1);

        curSpeed = movementSpeed * 2;
        knockBackMovement = true;

        yield return new WaitForSeconds(knockDownTime);

        knockBackMovement = false;
        canAttack = true;
        curSpeed = 0f;
        focus = Random.Range(1, 4);
    }

    IEnumerator ArrowHit()
    {
        focus = 0; // Stop

        yield return new WaitForSeconds(knockDownTime);
    }

}
