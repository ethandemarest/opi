using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using EZCameraShake;

public class PlayerController : MonoBehaviour
{
    CameraFollow cameraFollow;
    bool playerController;
    public GameObject currentObject = null;
    public GameObject opiSlashOne;
    public GameObject opiSlashTwo;
    GameObject hitbox;
    GameObject opiCenter;
    public GameObject reticle;
    public GameObject arrowPrefab;
    public GameObject bow;

    //Stamina
    public GameObject staminaBar;
    public float stamina;
    
    SpriteRenderer sprite;

    //Movement
    Rigidbody2D rb;
    Animator animator;

    [Header("||Movement||")]
    public float speed = 10f;
    public Vector2 movement, lastMove;

    Vector3 knockBack;
    Vector2 lockDirection;
    Vector3 difference;

    //Atack
    [Header("||Combat||")]
    public float arrowSpeed = 70f;
    public Transform spawnPoint;
    bool attack;
    bool canAttack;
    bool attacking;
    public float damageKnockBack;
    public float swingForce;
    public float knockDownTime;

    public int noOfClicks = 0;
    public float maxComboDelay = 0.5f;
    public float attackDelay;
    float lastClickedTime = 0;

    [HideInInspector]
    public bool bowReady;
    bool projectile;
    bool arrowReady;
    bool draw;

    //Roll
    [Header("||Roll||")]
    public float rollSpeed;
    public float rollDuration = 1f;
    bool invincible;

    [HideInInspector]
    public bool rolling;
    int inputSource;
    public bool roll;
    public bool canRoll;

    //Interact
    [HideInInspector]
    public bool sceneTrigger, canInteract, hit;
    [HideInInspector]
    public bool wasHit;
    bool atCauldron;

    public int behavior;

    // Update is called once per frame
    void Start()
    {
        cameraFollow = GameObject.Find("Camera Holder").GetComponent<CameraFollow>();
        opiCenter = GameObject.Find("Opi Center");
        playerController = true;
        bowReady = true;

        hitbox = GameObject.Find("Opi Sword Hitbox");
        sprite = GetComponent<SpriteRenderer>();
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        rolling = false;
        canInteract = true;
        canRoll = true;
        attacking = false;
        canAttack = true;
        wasHit = false; 
        inputSource = 0;
        lastMove.y = -1f;
        behavior = 0;
    }

    public void Update()
    {
        //// INPUT ////
        stamina = staminaBar.GetComponent<staminaBar>().currentStamina;


        //Movement
        if (playerController == true)
        {
            movement.x = Input.GetAxisRaw("Horizontal"); //MOVEMENT
            movement.y = Input.GetAxisRaw("Vertical");
            roll = Input.GetButtonDown("roll"); //ROLL
            attack = Input.GetButtonDown("attack"); //SWORD CONTROLS

            if (Input.GetButtonDown("bow")){  
                draw = true;}
            if (Input.GetButtonUp("bow")){
                draw = false;}

        }
        else
        {
            movement.x = 0;
            movement.y = 0;
            roll = false;
            attack = false;
            draw = false;
        }

        //Roll

        if (roll && canRoll == true && bowReady == true && stamina > 2)
        {
            StartCoroutine(Rolling(lastMove, 4f));
        }

        //Attack
        if (Time.time - lastClickedTime > maxComboDelay) { noOfClicks = 0; }

        if (attack && canAttack == true && behavior <= 1)
        {
            lastClickedTime = Time.time;
            noOfClicks++;
            if (noOfClicks % 2 == 1)
            {
                StopAllCoroutines();
                StartCoroutine(AttackOne(lastMove, 2f));
            }
            if (noOfClicks % 2 == 0)
            {
                StopAllCoroutines();
                StartCoroutine(AttackTwo(lastMove, 2f));
            }
        }

        if (rolling == false && attacking == false)
        {
            if (draw && bowReady == true){
                StopAllCoroutines();
                StartCoroutine("BowDraw"); }
            if (!draw && arrowReady){
                StopAllCoroutines();
                StartCoroutine("BowShoot");}
        }
        

        //Movement
        Vector3[] input = new Vector3[2];
        input[0] = new Vector3(movement.x, movement.y, 0f);
        input[1] = new Vector3(lockDirection.x, lockDirection.y, 0f);

        animator.SetFloat("Horizontal", input[inputSource].x);
        animator.SetFloat("Vertical", input[inputSource].y);

        //Last Move
        if (movement.x > 0.02 || movement.x < -0.02 || movement.y > 0.02 || movement.y < -0.02)
        {
            lastMove.x = movement.x;
            lastMove.y = movement.y;
            animator.SetFloat("Last Move Horizontal", lastMove.x);
            animator.SetFloat("Last Move Vertical", lastMove.y);
        }
            
    }

    private void FixedUpdate()
    {
        if(behavior == 0) // MOVEMENT 
        {
            rb.MovePosition(rb.position + movement.normalized * speed * Time.fixedDeltaTime);

            if(movement.x != 0 || movement.y != 0)
            {
                animator.SetFloat("Speed", 1);
            }
            else if (movement.x == 0 || movement.y == 0)
            {
                animator.SetFloat("Speed", 0);
            }
        }
        else if(behavior == 1) // ATTACKING
        {
            transform.position = Vector2.Lerp(transform.position, knockBack, 0.05f);
        }
        else if(behavior == 2) // ROLLING
        {
            rb.MovePosition(rb.position + lockDirection * rollSpeed * Time.fixedDeltaTime);
        }
        else if(behavior == 3) // HIT
        {
            rb.position = Vector2.Lerp(transform.position, knockBack, 0.05f);
        }else if(behavior == 4) // BOW
        {
            transform.position = transform.position;
        }else if(behavior == 5) // BOUNCE
        {
            transform.position = Vector2.Lerp(transform.position, knockBack, 0.05f);
        }

    }

    //TRIGGERS
    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("EnemyDamage") && invincible == false)
        {
            SendMessage("TakeDamage", 1);
            projectile = false;
            currentObject = other.gameObject;
            StartCoroutine("Hit", (other.gameObject, name));
        }
        if (other.CompareTag("EnemySpell") && invincible == false)
        {
            SendMessage("TakeDamage", 1);
            projectile = true;
            currentObject = other.gameObject;
            StartCoroutine("Hit", (other.gameObject,name));
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if(rolling == true && collision.gameObject.CompareTag("Environment"))
        {
            StartCoroutine("Bounce", lockDirection.normalized);
        }
    }

    public void AddIngredient()
    {
        animator.SetBool("Scene Trigger", true);
    }

    public void Death()
    {
        StopAllCoroutines();
        FindObjectOfType<AudioManager>().Play("Arrow Impact");
        playerController = false;
    }

    //ENUMERATORS

    IEnumerator AttackOne(Vector2 attackDir, float attackCost)
    {
        if (stamina < attackCost)
        {
            print("break");
            behavior = 0;
            yield break;
        }

        SendMessage("SlashOne");
        hitbox.SendMessage("Attack");
        staminaBar.SendMessage("UseEnergy", 2);
        animator.SetBool("Attack 1", true);


        string[] opiSound = new string[3];
        opiSound[0] = ("Opi Voice Swing 1");
        opiSound[1] = ("Opi Voice Swing 2");
        opiSound[2] = ("Opi Voice Swing 3");
        FindObjectOfType<AudioManager>().Play(opiSound[Random.Range(0, 3)]);
        FindObjectOfType<AudioManager>().Play("Sword Swing");

        behavior = 1;
        canAttack = false;
        attacking = true;
        lockDirection = attackDir;
        animator.SetFloat("Lock Direction X", lockDirection.x);
        animator.SetFloat("Lock Direction Y", lockDirection.y);
        animator.SetFloat("Speed", 0);

        knockBack.x = transform.position.x + attackDir.x * swingForce;
        knockBack.y = transform.position.y + attackDir.y * swingForce;

        inputSource = 1;

        yield return new WaitForSeconds(attackDelay);

        canAttack = true;

        yield return new WaitForSeconds(attackDelay);

        canRoll = true;
        attacking = false;
        inputSource = 0;
        behavior = 0;
    }

    IEnumerator AttackTwo(Vector2 attackDir, float attackCost)
    {
        if (stamina < attackCost)
        {
            print("break");
            behavior = 0;
            yield break;
        }

        SendMessage("SlashTwo");
        hitbox.SendMessage("Attack");
        animator.SetBool("Attack 2", true);
        staminaBar.SendMessage("UseEnergy", 2);

        string[] opiSound = new string[3];
        opiSound[0] = ("Opi Voice Swing 1");
        opiSound[1] = ("Opi Voice Swing 2");
        opiSound[2] = ("Opi Voice Swing 3");
        FindObjectOfType<AudioManager>().Play(opiSound[Random.Range(0, 3)]);
        FindObjectOfType<AudioManager>().Play("Sword Swing");

        behavior = 1;
        canAttack = false;
        attacking = true;
        lockDirection = attackDir;
        animator.SetFloat("Lock Direction X", lockDirection.x);
        animator.SetFloat("Lock Direction Y", lockDirection.y);
        animator.SetFloat("Speed", 0);

        knockBack.x = transform.position.x + attackDir.x * swingForce;
        knockBack.y = transform.position.y + attackDir.y * swingForce;

        inputSource = 1;

        yield return new WaitForSeconds(attackDelay);

        canAttack = true;

        yield return new WaitForSeconds(attackDelay);

        canRoll = true;
        attacking = false;
        inputSource = 0;
        behavior = 0;

    }

    IEnumerator Rolling(Vector2 rollDir, float rollCost)
    {
        if(stamina < rollCost)
        {
            yield break;
        }

        animator.SetBool("Roll", roll);
        staminaBar.SendMessage("UseEnergy", 4);

        gameObject.layer = 9;
        lockDirection = lastMove.normalized;

        invincible = true;
        rolling = true;
        canRoll = false;
        lockDirection = lastMove.normalized;
        inputSource = 1;
        behavior = 2;


        yield return new WaitForSeconds(rollDuration);

        gameObject.layer = 8;
        invincible = false;
        rolling = false;
        inputSource = 0;
        behavior = 0;
        canRoll = true;
    }

    IEnumerator BowDraw()
    {
        bow.SendMessage("BowDraw");
        bowReady = false;
        attacking = false;

        animator.SetBool("Bow", true);
        FindObjectOfType<AudioManager>().Play("Bow Draw");
        behavior = 4;

        yield return new WaitForSeconds(0.4f);

        arrowReady = true;       
    }

    IEnumerator BowShoot()
    {
        bow.SendMessage("BowDone");
        SendMessage("ArrowShoot");
        FindObjectOfType<AudioManager>().Play("Bow Shoot");

        arrowReady = false;

        animator.SetBool("Bow", false);
        animator.SetBool("Shoot", true);

        lockDirection = lastMove.normalized;    
        inputSource = 1;
        animator.SetFloat("Lock Direction X", lockDirection.x);
        animator.SetFloat("Lock Direction Y", lockDirection.y);

        yield return new WaitForSeconds(0.01f);

        animator.SetBool("Shoot", false);

        yield return new WaitForSeconds(0.5f);

        animator.SetBool("Shoot", false);
        inputSource = 0;
        bowReady = true;    
        behavior = 0;
     }

    public void ArrowShoot()
    {
        GameObject arrow = Instantiate(arrowPrefab, spawnPoint.position, spawnPoint.rotation);
        Rigidbody2D arrowRb = arrow.GetComponent<Rigidbody2D>();
        arrowRb.AddForce(spawnPoint.up * arrowSpeed, ForceMode2D.Impulse);
    }

    IEnumerator Hit()
    {
        CameraShaker.Instance.ShakeOnce(3f, 3f, .1f, 1f);
        SendMessage("Freeze");
        animator.SetBool("Hit", true);
        sprite.color = new Color(1, 1, 1, 0.5f);

        string[] opiHurt = new string[2];
        opiHurt[0] = ("Opi Hurt 1");
        opiHurt[1] = ("Opi Hurt 2");
        FindObjectOfType<AudioManager>().Play(opiHurt[Random.Range(0, 2)]);
        FindObjectOfType<AudioManager>().Play("Arrow Impact");

        invincible = true;
        if (projectile == true)
        {
            difference = (transform.position - currentObject.GetComponent<Transform>().position) + new Vector3(0f, 1.5f, 0f); 
        }
        else
        {
            difference = (opiCenter.transform.position - currentObject.GetComponent<Transform>().position);
        }
        
        knockBack.x = transform.position.x + difference.normalized.x * damageKnockBack;
        knockBack.y = transform.position.y + difference.normalized.y * damageKnockBack;
            
        wasHit = true;
        canRoll = false;
        behavior = 3;

        yield return new WaitForSeconds(knockDownTime);

        invincible = false;
        sprite.color = new Color(1, 1, 1, 1);
        wasHit = false;
        canRoll = true;
        behavior = 0;
    }

    IEnumerator Bounce(Vector2 bounceDir)
    {
        CameraShaker.Instance.ShakeOnce(3f, 3f, .1f, 1f);
        animator.SetBool("Hit", true);
        sprite.color = new Color(1, 1, 1, 0.5f);

        string[] opiHurt = new string[2];
        opiHurt[0] = ("Opi Hurt 1");
        opiHurt[1] = ("Opi Hurt 2");
        FindObjectOfType<AudioManager>().Play(opiHurt[Random.Range(0, 2)]);
        FindObjectOfType<AudioManager>().Play("Arrow Impact");

        knockBack.x = transform.position.x - (bounceDir.x * 3);
        knockBack.y = transform.position.y - (bounceDir.y * 3);

        wasHit = true;
        canRoll = false;
        behavior = 5;

        yield return new WaitForSeconds(knockDownTime);

        invincible = false;
        sprite.color = new Color(1, 1, 1, 1);
        wasHit = false;
        canRoll = true;
        behavior = 0;
    }

    public void SlashOne()
    {
        Instantiate(opiSlashOne, opiCenter.transform.position + new Vector3(lastMove.x*1.5f, (lastMove.y*1.5f)+0.7f), Quaternion.Euler(0f, 0f, 0f));
    }
    public void SlashTwo()
    {
        Instantiate(opiSlashTwo, opiCenter.transform.position + new Vector3(lastMove.x * 1.5f, (lastMove.y * 1.5f)+0.7f), Quaternion.Euler(0f, 0f, 0f));
    }

}