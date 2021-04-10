using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using EZCameraShake;

public class EnemyShooterMovement : MonoBehaviour
{
    public bool startAlive;

    [Header("||Game Objects||")]
    public GameObject healthBar;
    public GameObject projectile;
    public GameObject spawner;
    public GameObject damageSpawner;
    public GameObject damage;
    public GameObject dust;
    GameObject opi;
    GameObject opiCenter;
    GameObject enemCenter;

    [Header("||Movement||")]
    public float opiDetectRange;
    public float movementSpeed = 50f;
    public float curSpeed = 0.0f;
    public float knockBackPower = 3;
    public float knockDownTime = 0.6f;
    public float teleportDelay;

    [Header("||Combat||")]
    public float attackRange = 5.0f;
    public float retreatRange;
    public float teleportRange;

    PlayerController pc;
    EnemyShooterMovement enemyShooter;
    SpriteRenderer sprite;
    Animator animator;

    Vector2 aim;
    Vector2 surroundBack;
    Vector2 surroundSide;
    Vector2 knockBack;
    Vector2 opiLastMove;
    Vector2 teleportLocation;

    float angle;
    int focus;
    bool knockBackMovement;
    bool canAttack;
    bool canTeleport;
    bool teleporting;


    // Update is called once per frame
    void Start()
    {
        if(startAlive == false){
            gameObject.SetActive(false);
        }

        canTeleport = true;
        enemyShooter = GetComponent<EnemyShooterMovement>();
        sprite = GetComponent<SpriteRenderer>();
        focus = 1;
        knockBackMovement = false;
        canAttack = true;
        animator = GetComponent<Animator>();

        opi = GameObject.Find("Opi");
        pc = GameObject.Find("Opi").GetComponent<PlayerController>();
        opiCenter = GameObject.Find("Opi Center");
        enemCenter = transform.GetChild(0).gameObject;
    }

    public void FixedUpdate()
    {
        surroundBack.x = (opi.transform.position.x - pc.lastMove.x * 2);
        surroundBack.y = (opi.transform.position.y - pc.lastMove.y * 2);

        surroundSide.x = opi.transform.position.x + pc.lastMove.y * 2;
        surroundSide.y = opi.transform.position.y + pc.lastMove.x * 2;

        //BEHAVIOR LIST
        Vector2[] targetPosition = new Vector2[4];  
        targetPosition[0] = transform.position; //Enemy
        targetPosition[1] = opiCenter.transform.position; //Opi
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
    
        //ANIMATION
        animator.SetFloat("Horizontal", -aim.x);
        animator.SetFloat("Vertical", -aim.y);

        if (knockBackMovement == true) // KNOCK BACK MOVEMENT
        {
            print("knockback");
            if (curSpeed > 0){
                curSpeed = curSpeed - 1f;}

            transform.position = Vector2.MoveTowards(transform.position, knockBack, curSpeed * Time.deltaTime);
        }

        else if (opiDistance <= opiDetectRange && canAttack == true) // CHASE
        {
            print("chase");
            if (curSpeed < 8){
                curSpeed = curSpeed + 1f;}

            transform.position = Vector2.MoveTowards(transform.position, targetPosition[focus], curSpeed * Time.deltaTime);
            animator.SetFloat("Speed", curSpeed);
        }


        //RETREAT
        if (opiDistance <= retreatRange && canAttack && canTeleport)
        {
            StartCoroutine("Teleport");
        }
        else if (opiDistance <= attackRange && canAttack){
            StartCoroutine("Attack");
        }

        if(teleporting == true)
        {
            if (curSpeed > 0)
            {
                curSpeed = curSpeed - 0.5f;
            }
            transform.position = Vector2.MoveTowards(transform.position, teleportLocation, curSpeed * Time.deltaTime);
        }
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("OpiDamage"))
        {
            StopAllCoroutines();
            StartCoroutine("SwordHit", 15);
            SendMessage("TakeDamage", 1);

            FindObjectOfType<AudioManager>().Play("Sword Hit");
            FindObjectOfType<AudioManager>().Play("Enemy Hurt");
        }

        if (other.CompareTag("Arrow"))
        {
            StopAllCoroutines();
            StartCoroutine("SwordHit", 15);
            SendMessage("TakeDamage", 1);

            FindObjectOfType<AudioManager>().Play("Sword Hit");
            FindObjectOfType<AudioManager>().Play("Enemy Hurt");
            FindObjectOfType<AudioManager>().Play("Spellcaster Deflect");
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Environment"))
        {
            print("collision");
            teleportLocation.x = opiCenter.transform.position.x + (pc.lastMove.x * 10);
            teleportLocation.y = opiCenter.transform.position.y + (pc.lastMove.y * 10);
        }
    }

    public void Death()
    {
        StopAllCoroutines();
        enemyShooter.enabled = false;
    }


    IEnumerator Attack()
    {
        focus = 0;
        curSpeed = 0;
        animator.SetBool("Attack", true);

        canAttack = false;

        yield return new WaitForSeconds(0.9f);
        Instantiate(projectile, spawner.transform.position, Quaternion.Euler(0f,0f,angle));
        Instantiate(damage, spawner.transform.position, Quaternion.Euler(0f, 0f, angle));
        animator.SetBool("AttackShort", true);

        yield return new WaitForSeconds(0.5f);
        Instantiate(projectile, spawner.transform.position, Quaternion.Euler(0f, 0f, angle));
        Instantiate(damage, spawner.transform.position, Quaternion.Euler(0f, 0f, angle));
        animator.SetBool("AttackShort", true);

        yield return new WaitForSeconds(0.5f);
        Instantiate(projectile, spawner.transform.position, Quaternion.Euler(0f, 0f, angle));
        Instantiate(damage, spawner.transform.position, Quaternion.Euler(0f, 0f, angle));

        yield return new WaitForSeconds(0.5f);

        focus = Random.Range(1,4);
        curSpeed = 0;
        canAttack = true;
    }

    IEnumerator Teleport()
    {
        animator.SetBool("Teleport", true);
        canAttack = false;
        canTeleport = false;
        gameObject.layer = 15;


        yield return new WaitForSeconds(0.9f);


        curSpeed = 25;
        teleporting = true;

        print("teleporting");
        teleportLocation.x = opiCenter.transform.position.x - (pc.lastMove.x * 10);
        teleportLocation.y = opiCenter.transform.position.y - (pc.lastMove.y * 10);
        sprite.color = new Color(1, 1, 1, 0);

        Instantiate(dust,transform.position, Quaternion.Euler(0f, 0f, 0f));
        yield return new WaitForSeconds(1f);

        Instantiate(dust, transform.position, Quaternion.Euler(0f, 0f, 0f));
        sprite.color = new Color(1, 1, 1, 1);
        animator.SetBool("TeleportIn", true);

        gameObject.layer = 11;
        teleporting = false;

        yield return new WaitForSeconds(1f);

        curSpeed = 0;
        canAttack = true;


        yield return new WaitForSeconds(teleportDelay);
        canTeleport = true;

    }

    IEnumerator SwordHit(float intensity)
    {
        canAttack = false;
        animator.SetBool("Hit", true);
        CameraShaker.Instance.ShakeOnce(2f, 2f, .1f, 1f);

        focus = 0; // Stop
        opiLastMove.x = pc.lastMove.x;
        opiLastMove.y = pc.lastMove.y;
        knockBack.x = (transform.position.x + opiLastMove.x * knockBackPower);
        knockBack.y = (transform.position.y + opiLastMove.y * knockBackPower);
        SendMessage("DamageEffect");

        sprite.color = new Color(1, 1, 1, 0.5f);

        yield return new WaitForSeconds(0.15f);

        knockBackMovement = true;
        curSpeed = intensity;

        sprite.color = new Color(1, 1, 1, 1);

        yield return new WaitForSeconds(knockDownTime);

        knockBackMovement = false;
        canAttack = true;
        canTeleport = true;
        curSpeed = 0f;
        focus = Random.Range(1, 4);
    }

    IEnumerator ArrowHit()
    {
        focus = 0; // Stop

        yield return new WaitForSeconds(knockDownTime);
    }

}
