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

    //Health
    public float maxHealth = 10;
    public float currentHealth;
    public HealthBar healthBar;

    //Movement
    Rigidbody2D rb;
    Animator animator;
    public float speed = 10f;
    public float rollDelay = 1f;
    public float inputX;
    public float inputY;

    private Vector3 knockBack;
    public Vector2 movement;
    private Vector2 stopSpeed;
    public Vector2 lastMove;
    private Vector2 rollAngle;
    private Vector2 rollDirection;

    public float lastMoveX;
    public float lastMoveY;

    //Atack
    bool canAttack;
    public float knockBackPower;

    public int noOfClicks = 0;
    float lastClickedTime = 0;
    public float maxComboDelay = 0.5f;
    public bool hasAttacked;
    public bool attacking;
    public float attackDelay;

    //Roll
    int inputSource;
    public bool rolling;
    bool roll;
    bool canRoll;
    public float rollBoost;
    private Vector2 velocity = Vector2.zero;

    //Interact
    public bool sceneTrigger;
    public bool interact;
    bool hit;
    bool wasHit;

    // Update is called once per frame
    void Start()
    {
        currentHealth = maxHealth;
        healthBar.SetMaxHealth(maxHealth);

        hitbox = GameObject.Find("Hitbox");

        rb = this.GetComponent<Rigidbody2D>();
        animator = this.GetComponent<Animator>();
        rolling = false;
        canRoll = true;
        canAttack = true;
        wasHit = false;
        inputSource = 0;
        lastMoveY = -2f;
    }

    public void Update()
    {
        //// INPUT ////

        //Movement
        inputX = Input.GetAxisRaw("Horizontal");
        inputY = Input.GetAxisRaw("Vertical");

        movement.x = inputX;
        movement.y = inputY;

        stopSpeed.x = 0;
        stopSpeed.y = 0;

        //Roll
        roll = Input.GetButtonDown("roll");

        if (roll && canRoll == true)
        {
            canRoll = false;
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
        if (Input.GetButtonDown("attack") && rolling == false && canAttack == true && wasHit == false)
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

        //Item
        interact = Input.GetButtonDown("interact");

        //Movement

        Vector3[] input = new Vector3[2];
        input[0] = new Vector3(movement.x, movement.y, 0f);
        input[1] = new Vector3(rollDirection.x, rollDirection.y, 0f);

        animator.SetFloat("Horizontal", input[inputSource].x);
        animator.SetFloat("Vertical", input[inputSource].y);
        animator.SetFloat("Speed", movement.sqrMagnitude);

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
    }

    
    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Damage") && wasHit == false && rolling == false)
        {
            CameraShaker.Instance.ShakeOnce(4f, 4f, .1f, 1f);
            currentObject = other.gameObject;
            StartCoroutine("Hit");
            TakeDamage(2);
        }
        else
        {
            currentObject = null;
        }
    }

    public void TakeDamage(float damage)
    {
        currentHealth -= damage;
        healthBar.SetHealth(currentHealth);
    }
    
    public void AddIngredient()
    {
        animator.SetBool("Scene Trigger", true);
    }

    //ENUMERATORS

    IEnumerator Rolling()
    {
        rollAngle = transform.position;
        rollDirection = lastMove.normalized;
        rolling = true;
        inputSource = 1;

        yield return new WaitForSeconds(rollDelay);

        rolling = false;
        canRoll = true;
        inputSource = 0;
    }

    IEnumerator AttackOne()
    {
        SendMessage("SlashOne");
        attacking = true;
        animator.SetBool("Attack 1", true);

        canAttack = false;
        rollDirection = lastMove.normalized;
        inputSource = 1;

        yield return new WaitForSeconds(attackDelay);

        attacking = false;
        canAttack = true;
        inputSource = 0;
    }

    IEnumerator AttackTwo()
    {
        SendMessage("SlashTwo");
        attacking = true;
        animator.SetBool("Attack 2", true);

        canAttack = false;
        rollDirection = lastMove.normalized;
        inputSource = 1;

        yield return new WaitForSeconds(attackDelay);

        attacking = false;
        canAttack = true;
        inputSource = 0;
    }

    IEnumerator Hit()
    {
        Vector3 difference = (transform.position - currentObject.GetComponent<Transform>().position);
        animator.SetBool("Hit", true);

        knockBack.x = transform.position.x + difference.normalized.x * knockBackPower;
        knockBack.y = transform.position.y + difference.normalized.y * knockBackPower;

        wasHit = true;
        canRoll = false;

        yield return new WaitForSeconds(0.5f);
        
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

    public void FixedUpdate()
    {
        //// MOVEMENT ////

        // Attack Cancel Move
        if (this.animator.GetCurrentAnimatorStateInfo(0).IsName("Attack 1") || this.animator.GetCurrentAnimatorStateInfo(0).IsName("Attack 2"))
        {
            movement = stopSpeed;
        }
        if (this.animator.GetCurrentAnimatorStateInfo(0).IsName("ItemDrop"))
        {
            movement = stopSpeed;
        }
        

        //Movement Expression

        if (rolling == true)
        {
            rb.MovePosition(rb.position +  rollDirection * rollBoost * Time.fixedDeltaTime);
        }

        if (wasHit == true)
        {
            rb.MovePosition(Vector2.Lerp(transform.position, knockBack, 0.05f));
        }
        else if (rolling == false)
        {
            rb.MovePosition(rb.position + movement.normalized * speed * Time.fixedDeltaTime);
        }
    }
}


