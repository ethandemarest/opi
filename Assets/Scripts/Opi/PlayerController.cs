using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using EZCameraShake;

public class PlayerController : MonoBehaviour
{
    bool playerController;
    public GameObject currentObject = null;
    public GameObject opiSlashOne;
    public GameObject opiSlashTwo;
    GameObject hitbox;
    GameObject opiCenter;
    public GameObject reticle;
    public GameObject arrowPrefab;
    

    SpriteRenderer sprite;

    //Movement
    Rigidbody2D rb;
    Animator animator;

    [Header("||Movement||")]
    public float speed = 10f;
    [HideInInspector]
    public Vector2 movement, lastMove;
    int targetSpeed;

    Vector3 knockBack;
    Vector3 offset;
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
    public float knockBackFromHittingEnemy;
    public float knockDownTime;

    public int noOfClicks = 0;
    public float maxComboDelay = 0.5f;
    public float attackDelay;
    float lastClickedTime = 0;

    bool projectile;
    bool arrowReady;
    bool bowReady;
    bool draw;
    bool enemyContact;

    //Roll
    [Header("||Roll||")]
    public float rollSpeed;
    public float rollDuration = 1f;
    public float rollRecharge;
    bool invincible;

    [HideInInspector]
    public bool rolling;
    int inputSource;
    bool roll;
    bool canRoll;

    //Interact
    [HideInInspector]
    public bool sceneTrigger, interact, canInteract, hit;
    [HideInInspector]
    public bool wasHit;
    bool atCauldron;    

    // Update is called once per frame
    void Start()
    {
        opiCenter = GameObject.Find("Opi Center");
        playerController = true;
        bowReady = true;

        /*
        currentHealth = maxHealth;
        healthBar.SetMaxHealth(maxHealth);
        */

        hitbox = GameObject.Find("Opi Sword Hitbox");
        sprite = GetComponent<SpriteRenderer>();
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        rolling = false;
        atCauldron = false;
        canInteract = true;
        canRoll = true;
        attacking = false;
        canAttack = true;
        wasHit = false; 
        inputSource = 0;
        targetSpeed = 0;
        lastMove.y = -2f;

        offset = -(transform.position - opiCenter.transform.position);
    }

    public void Update()
    {
        //// INPUT ////

        //Movement
        if(playerController == true)
        {
            movement.x = Input.GetAxisRaw("Horizontal"); //MOVEMENT
            movement.y = Input.GetAxisRaw("Vertical");
            roll = Input.GetButtonDown("roll"); //ROLL
            attack = Input.GetButtonDown("attack"); //SWORD CONTROLS
            if (Input.GetButtonDown("bow")) //BOW CONTROLS
            {  
                draw = true;
            }
            if (Input.GetButtonUp("bow")){
                draw = false;
            }
            interact = Input.GetButtonDown("interact"); //ITEM CONTROLS
        }
        else
        {
            targetSpeed = 1;
            movement.x = lastMove.x;
            movement.y = lastMove.y;
            attack = false;
            interact = false;
        }


        //Roll
        if (roll && canRoll == true && bowReady == true)
        {
            animator.SetBool("Roll", roll);
            StartCoroutine("Rolling");
        }

        //Attack
        if (Time.time - lastClickedTime > maxComboDelay)
        {
            noOfClicks = 0;
        }


        if (attack
            && rolling == false
            && canAttack == true
            && wasHit == false)
        {
            lastClickedTime = Time.time;
            noOfClicks++;
            if (noOfClicks % 2 == 1)
            {
                hitbox.SendMessage("Attack");
                StartCoroutine("AttackOne");
                StartCoroutine("Swing");
                animator.SetBool("Attack 1", true);
            }
            if (noOfClicks % 2 == 0)
            {
                hitbox.SendMessage("Attack");
                StartCoroutine("AttackTwo");
                StartCoroutine("Swing");
                animator.SetBool("Attack 2", true);
            }
        }

        enemyContact = hitbox.GetComponent<Hitbox>().contact;


        Vector3 aim = new Vector3(lastMove.x, lastMove.y, 0.0f);
        aim.Normalize();
        reticle.transform.localPosition = new Vector3(0, 0, 0) + (aim * 3);

        if (rolling == false)
        {
            if (draw == true && bowReady == true)
            {
                StartCoroutine("BowDraw");
            }
            if (draw == false && arrowReady)
            {
                StartCoroutine("BowShoot");
            }
        }


        if (interact && canInteract == true && atCauldron == true)
        {
            StartCoroutine("Interact");
        }

        //Movement
        Vector3[] input = new Vector3[2];
        input[0] = new Vector3(movement.x, movement.y, 0f);
        input[1] = new Vector3(lockDirection.x, lockDirection.y, 0f);

        float[] moveSpeed = new float[2];
        moveSpeed[0] = speed;
        moveSpeed[1] = 0f;

        animator.SetFloat("Horizontal", input[inputSource].x);
        animator.SetFloat("Vertical", input[inputSource].y);
        animator.SetFloat("Speed", (movement.sqrMagnitude * moveSpeed[targetSpeed]));

        //Last Move
        if (Input.GetAxisRaw("Horizontal") > 0.1 || Input.GetAxisRaw("Horizontal") < -0.1 || Input.GetAxisRaw("Vertical") > 0.1 || Input.GetAxisRaw("Vertical") < -0.1)
        {
            lastMove.x = input[inputSource].x;
            lastMove.y = input[inputSource].y;
            animator.SetFloat("Last Move Horizontal", lastMove.x*2); 
            animator.SetFloat("Last Move Vertical", lastMove.y*2);
        }

        //Movement Expression
        if(attacking == true)
        {
            transform.position = Vector2.Lerp(transform.position, knockBack, 0.05f);
        }
        if (rolling == true){
            rb.MovePosition(rb.position + lockDirection * rollSpeed * Time.fixedDeltaTime);
        }
        if (wasHit == true){
            transform.position = Vector2.Lerp(transform.position, knockBack, 0.05f);
        }
        else if (rolling == false && bowReady == true){
            rb.MovePosition(rb.position + movement.normalized * moveSpeed[targetSpeed] * Time.fixedDeltaTime);
            FindObjectOfType<AudioManager>().Play("Opi Footsteps");
        }
    }

    //TRIGGERS
    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Scene")){
            atCauldron = true;
        }
        if (other.CompareTag("Damage") && invincible == false)
        {
            projectile = false;
            currentObject = other.gameObject;
            StartCoroutine("Hit", (other.gameObject, name));
        }
        if (other.CompareTag("EnemySpell") && invincible == false)
        {
            projectile = true;
            currentObject = other.gameObject;
            StartCoroutine("Hit", (other.gameObject,name));
        }
    }
    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Scene")){
            atCauldron = false;
        }
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Enemy") && wasHit == false && rolling == false)
        {
            CameraShaker.Instance.ShakeOnce(3f, 3f, .1f, 1f);
            currentObject = collision.gameObject;
            StartCoroutine("Hit");
            //TakeDamage(2);
        }
    }
    /*
    public void TakeDamage(float damage)
    {
        currentHealth -= damage;
        healthBar.SetHealth(currentHealth);
    }
    */
    public void AddIngredient()
    {
        animator.SetBool("Scene Trigger", true);
    }


    //ENUMERATORS

    IEnumerator AttackOne()
    {
        if (enemyContact == true)
        {
            StartCoroutine("Contact");
        }

        attacking = true;

        string[] opiSound = new string[3];
        opiSound[0] = ("Opi Voice Swing 1");
        opiSound[1] = ("Opi Voice Swing 2");
        opiSound[2] = ("Opi Voice Swing 3");
        FindObjectOfType<AudioManager>().Play(opiSound[Random.Range(0,3)]);

        lockDirection = lastMove.normalized;
        canAttack = false;
        inputSource = 1;
        targetSpeed = 1;
        SendMessage("SlashOne");
        FindObjectOfType<AudioManager>().Play("Sword Swing");

        yield return new WaitForSeconds(attackDelay);

        attacking = false;
        canAttack = true;

        yield return new WaitForSeconds(0.3f);
        if (!animator.GetCurrentAnimatorStateInfo(0).IsName("Attack 2"))
        {
            inputSource = 0;
            targetSpeed = 0;
        }
    }

    IEnumerator AttackTwo()
    {
        if (enemyContact == true)
        {
            StartCoroutine("Contact");
        }
        attacking = true;

        string[] opiSwing = new string[3];
        opiSwing[0] = ("Opi Voice Swing 1");
        opiSwing[1] = ("Opi Voice Swing 2");
        opiSwing[2] = ("Opi Voice Swing 3");
        FindObjectOfType<AudioManager>().Play(opiSwing[Random.Range(0, 3)]);

        lockDirection = lastMove.normalized;
        canAttack = false;
        inputSource = 1;
        targetSpeed = 1;
        SendMessage("SlashTwo");
        FindObjectOfType<AudioManager>().Play("Sword Swing");

        yield return new WaitForSeconds(attackDelay);

        attacking = false;
        canAttack = true;

        yield return new WaitForSeconds(0.3f);
        if (!this.animator.GetCurrentAnimatorStateInfo(0).IsName("Attack 1"))
        {
            inputSource = 0;
            targetSpeed = 0;
        }
    }

    IEnumerator Interact()
    {
        canInteract = false;
        inputSource = 1;
        targetSpeed = 1;

        yield return new WaitForSeconds(1.8f);

        canInteract = true;
        inputSource = 0;
        targetSpeed = 0;
    }

    IEnumerator Rolling()
    {
        invincible = true;
        rolling = true;
        canRoll = false;
        lockDirection = lastMove.normalized;
        inputSource = 1;

        yield return new WaitForSeconds(rollDuration);

        invincible = false;
        rolling = false;
        inputSource = 0;

        yield return new WaitForSeconds(rollRecharge);

        canRoll = true;
    }   

    IEnumerator BowDraw()
    {
        bowReady = false;
        attacking = false;
        targetSpeed = 1;

        animator.SetBool("Bow", true);
        FindObjectOfType<AudioManager>().Play("Bow Draw");


        yield return new WaitForSeconds(0.4f);

        arrowReady = true;       

    }

    IEnumerator BowShoot()
    {
        SendMessage("ArrowShoot");
        FindObjectOfType<AudioManager>().Play("Bow Shoot");


        arrowReady = false;

        animator.SetBool("Bow", false);
        animator.SetBool("Shoot", true);

        lockDirection = lastMove.normalized;    
        inputSource = 1;
        targetSpeed = 1;

        yield return new WaitForSeconds(0.01f);

        animator.SetBool("Shoot", false);

        yield return new WaitForSeconds(0.5f);

        animator.SetBool("Shoot", false);
        inputSource = 0;
        targetSpeed = 0;
        bowReady = true;

    }

    public void ArrowShoot()
    {
        GameObject arrow = Instantiate(arrowPrefab, spawnPoint.position, spawnPoint.rotation);
        Rigidbody2D arrowRb = arrow.GetComponent<Rigidbody2D>();
        arrowRb.AddForce(spawnPoint.up * arrowSpeed, ForceMode2D.Impulse);
    }

    IEnumerator Hit()
    {
        SendMessage("DamageEffect");
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

        yield return new WaitForSeconds(knockDownTime);

        invincible = false;
        sprite.color = new Color(1, 1, 1, 1);
        wasHit = false;
        canRoll = true;
    }

    IEnumerator Swing()
    {
        knockBack.x = transform.position.x + lastMove.x * (knockBackFromHittingEnemy * 0.7f);
        knockBack.y = transform.position.y + lastMove.y * (knockBackFromHittingEnemy * 0.7f);

        yield return new WaitForSeconds(0.1f);

        wasHit = true;
        canRoll = false;

        yield return new WaitForSeconds(0.1f);

        wasHit = false;
        canRoll = true;
    }

    IEnumerator Contact()
    {
        knockBack.x = transform.position.x - lastMove.x * knockBackFromHittingEnemy;
        knockBack.y = transform.position.y - lastMove.y * knockBackFromHittingEnemy;

        yield return new WaitForSeconds(0.1f);

        wasHit = true;
        canRoll = false;

        yield return new WaitForSeconds(0.1f);

        wasHit = false;
        canRoll = true;
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
    



