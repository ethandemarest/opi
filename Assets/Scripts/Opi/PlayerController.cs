using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using EZCameraShake;

public class PlayerController : MonoBehaviour
{
    public GameObject currentObject = null;
    public GameObject opiSlashOne;
    public GameObject opiSlashTwo;
    GameObject hitbox;
    public GameObject reticle;
    public GameObject arrowPrefab;

    //Health
    /*
    public HealthBar healthBar;
    public float maxHealth = 10;
    public float currentHealth;
    */

    //Movement
    Rigidbody2D rb;
    Animator animator;

    [Header("||Movement||")]
    public float speed = 10f;
    public Vector2 movement;
    public Vector2 lastMove;

    Vector3 knockBack;
    Vector2 stopSpeed;
    Vector2 rollAngle;
    Vector2 lockDirection;

    public float lastMoveX;
    public float lastMoveY;

    //Atack
    [Header("||Attack||")]
    public float arrowSpeed = 20f;
    public Transform spawnPoint;
    bool attack;
    bool canAttack;
    public float damageKnockBack;
    public float knockBackFromHittingEnemy;

    int noOfClicks = 0;
    float lastClickedTime = 0;
    float maxComboDelay = 0.5f;
    bool hasAttacked;
    public float attackDelay;
    public int targetSpeed;

    bool bowDraw;
    bool arrowReady;
    bool bowReady;
    bool draw;
    bool enemyContact;
    GameObject currentEnemy = null;

    //Roll
    [Header("||Roll||")]
    public float rollSpeed;
    public float rollDelay = 1f;
    public float rollRecharge;

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
        bowReady = true;

        /*
        currentHealth = maxHealth;
        healthBar.SetMaxHealth(maxHealth);
        */

        hitbox = GameObject.Find("Hitbox");

        rb = this.GetComponent<Rigidbody2D>();
        animator = this.GetComponent<Animator>();
        rolling = false;
        atCauldron = false;
        canInteract = true;
        canRoll = true;
        canAttack = true;
        wasHit = false;
        inputSource = 0;
        targetSpeed = 0;
        lastMoveY = -2f;
    }

    public void Update()
    {
        //// INPUT ////

        //Movement

        movement.x = Input.GetAxisRaw("Horizontal");
        movement.y = Input.GetAxisRaw("Vertical");

        stopSpeed.x = 0;
        stopSpeed.y = 0;

        //Roll
        roll = Input.GetButtonDown("roll");

        if (roll && canRoll == true && bowReady == true)
        {
            animator.SetBool("Roll", roll);
            StartCoroutine("Rolling");
        }

        //Attack
        if (Time.time - lastClickedTime > maxComboDelay)
        {
            noOfClicks = 0;
            animator.SetBool("Attack 1", false);
            animator.SetBool("Attack 2", false);
        }

        if (Input.GetButtonDown("attack"))
        {
            attack = true;
        }
        else
        {
            attack = false;
        }

        if (attack && rolling == false && canAttack == true && wasHit == false)
        {
            lastClickedTime = Time.time;
            noOfClicks++;
            if (noOfClicks % 2 == 1)
            {
                hitbox.SendMessage("Attack");
                StartCoroutine("AttackOne");
            }
            if (noOfClicks % 2 == 0)
            {
                hitbox.SendMessage("Attack");
                StartCoroutine("AttackTwo");
            }
        }

        enemyContact = hitbox.GetComponent<Hitbox>().contact;
        currentEnemy = hitbox.GetComponent<Hitbox>().currentObject;

        if(enemyContact == true){
            StartCoroutine("Contact");
        }

        //BOW CONTROLS
        if (Input.GetButtonDown("bow")){
            draw = true;
        }
        if (Input.GetButtonUp("bow")){
            draw = false;
        }

        Vector3 aim = new Vector3(lastMoveX, lastMoveY, 0.0f);
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

        //Item
        interact = Input.GetButtonDown("interact");

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
            lastMoveX = input[inputSource].x + input[inputSource].x;
            lastMoveY = input[inputSource].y + input[inputSource].y;
            lastMove.x = input[inputSource].x;
            lastMove.y = input[inputSource].y;

            animator.SetFloat("Last Move Horizontal", lastMoveX);
            animator.SetFloat("Last Move Vertical", lastMoveY);
        }

        //Movement Expression  
        if (rolling == true){
            rb.MovePosition(rb.position + lockDirection * rollSpeed * Time.fixedDeltaTime);
        }
        if (wasHit == true){
            //rb.MovePosition(Vector2.MoveTowards(transform.position, knockBack, 0.05f));
            transform.position = Vector2.Lerp(transform.position, knockBack, 0.05f);
        }
        else if (rolling == false){
            rb.MovePosition(rb.position + movement.normalized * moveSpeed[targetSpeed] * Time.fixedDeltaTime);
        }

    }

    //TRIGGERS
    void OnTriggerEnter2D(Collider2D other)
    {
        /*
        //Damage + Camera Shake
        if (other.CompareTag("Enemy") && wasHit == false && rolling == false){
            CameraShaker.Instance.ShakeOnce(3f, 3f, .1f, 1f);
            currentObject = other.gameObject;
            StartCoroutine("Hit");
            //TakeDamage(2);
        }
        else
        {
            currentObject = null;
        }
        */

        if (other.CompareTag("Scene")){
            atCauldron = true;
        }
    }
    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Scene"))
        {
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
        rolling = true;
        canRoll = false;
        rollAngle = transform.position;
        lockDirection = lastMove.normalized;
        inputSource = 1;

        yield return new WaitForSeconds(rollDelay);

        rolling = false;
        inputSource = 0;

        yield return new WaitForSeconds(rollRecharge);

        canRoll = true;
    }   

    IEnumerator AttackOne()
    {
        lockDirection = lastMove.normalized;
        canAttack = false;

        inputSource = 1;
        targetSpeed = 1;

        SendMessage("SlashOne");
        animator.SetBool("Attack 1", true);
        FindObjectOfType<AudioManager>().Play("Sword Swing");

        yield return new WaitForSeconds(attackDelay);

        if (!this.animator.GetCurrentAnimatorStateInfo(0).IsName("Attack 2"))
        {
            inputSource = 0;
            targetSpeed = 0;
        }

        canAttack = true;

    }

    IEnumerator AttackTwo()
    {
        lockDirection = lastMove.normalized;
        canAttack = false;


        inputSource = 1;
        targetSpeed = 1;

        SendMessage("SlashTwo");
        animator.SetBool("Attack 2", true);
        FindObjectOfType<AudioManager>().Play("Sword Swing");

        yield return new WaitForSeconds(attackDelay);

        if (!this.animator.GetCurrentAnimatorStateInfo(0).IsName("Attack 1"))
        {
            inputSource = 0;
            targetSpeed = 0;
        }

        canAttack = true;
    }

    IEnumerator BowDraw()
    {
        animator.SetBool("Bow", true);
        FindObjectOfType<AudioManager>().Play("Bow Draw");

        bowReady = false;
        targetSpeed = 1;

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
        Vector3 difference = (transform.position - currentObject.GetComponent<Transform>().position);
        animator.SetBool("Hit", true);
        FindObjectOfType<AudioManager>().Play("Arrow Impact");

        knockBack.x = transform.position.x + difference.normalized.x * damageKnockBack;
        knockBack.y = transform.position.y + difference.normalized.y * damageKnockBack;

        print(knockBack);

        wasHit = true;
        canRoll = false;    

        yield return new WaitForSeconds(2.5f);
        
        wasHit = false;
        canRoll = true;
    }

    IEnumerator Contact()
    {
        knockBack.x = transform.position.x-lastMove.x * knockBackFromHittingEnemy;
        knockBack.y = transform.position.y-lastMove.y * knockBackFromHittingEnemy;

        yield return new WaitForSeconds(0.1f);

        wasHit = true;
        canRoll = false;

        yield return new WaitForSeconds(0.1f);

        wasHit = false;
        canRoll = true;
    }

    public void SlashOne()
    {
        Instantiate(opiSlashOne, this.transform.position + new Vector3(0f, 1.5f), Quaternion.Euler(0f, 0f, 0f));
    }
    public void SlashTwo()
    {
        Instantiate(opiSlashTwo, this.transform.position + new Vector3(0f, 1.5f), Quaternion.Euler(0f, 0f, 0f));
    }

}
    



