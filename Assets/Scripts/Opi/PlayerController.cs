﻿using System.Collections;
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
    public HealthBar healthBar;
    public float maxHealth = 10;
    public float currentHealth;

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

    public int targetSpeed;

    //Roll
    int inputSource;
    public bool rolling;
    public float rollRecharge;
    bool roll;
    bool canRoll;
    public float rollBoost;
    private Vector2 velocity = Vector2.zero;

    //Interact
    public bool sceneTrigger;
    public bool interact;
    bool canInteract;
    bool hit;
    public bool wasHit;
    bool atCauldron;

    // Update is called once per frame
    void Start()
    {
        currentHealth = maxHealth;
        healthBar.SetMaxHealth(maxHealth);

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

        if (interact && canInteract == true && atCauldron == true)
        {
            StartCoroutine("Interact");
        }

        //Movement
        Vector3[] input = new Vector3[2];
        input[0] = new Vector3(movement.x, movement.y, 0f);
        input[1] = new Vector3(rollDirection.x, rollDirection.y, 0f);

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
            rb.MovePosition(rb.position + rollDirection * rollBoost * Time.fixedDeltaTime);
        }
        if (wasHit == true){
            rb.MovePosition(Vector2.Lerp(transform.position, knockBack, 0.05f));
        }
        else if (rolling == false){
            rb.MovePosition(rb.position + movement.normalized * moveSpeed[targetSpeed] * Time.fixedDeltaTime);
        }
    }

    //TRIGGERS
    void OnTriggerEnter2D(Collider2D other)
    {
        //Damage + Camera Shake
        if (other.CompareTag("Damage") && wasHit == false && rolling == false){
            CameraShaker.Instance.ShakeOnce(4f, 4f, .1f, 1f);
            currentObject = other.gameObject;
            StartCoroutine("Hit");
            TakeDamage(2);
        }
        else
        {
            currentObject = null;
        }

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

    IEnumerator Interact()
    {
        canInteract = false;
        inputSource = 1;
        targetSpeed = 1;

        yield return new WaitForSeconds(2.0f);

        canInteract = true;
        inputSource = 0;
        targetSpeed = 0;
    }

    IEnumerator Rolling()
    {
        rolling = true;
        canRoll = false;
        rollAngle = transform.position;
        rollDirection = lastMove.normalized;
        inputSource = 1;

        yield return new WaitForSeconds(rollDelay);

        rolling = false;
        inputSource = 0;

        yield return new WaitForSeconds(rollRecharge);

        canRoll = true;
    }

    IEnumerator AttackOne()
    {
        rollDirection = lastMove.normalized;
        inputSource = 1;
        targetSpeed = 1;

        SendMessage("SlashOne");
        animator.SetBool("Attack 1", true);

        attacking = true;
        canAttack = false;

        yield return new WaitForSeconds(attackDelay);

        if (!this.animator.GetCurrentAnimatorStateInfo(0).IsName("Attack 2"))
        {
            inputSource = 0;
            targetSpeed = 0;
        }
        attacking = false;
        canAttack = true;
    }

    IEnumerator AttackTwo()
    {
        inputSource = 1;
        targetSpeed = 1;

        SendMessage("SlashTwo");
        animator.SetBool("Attack 2", true);

        attacking = true;
        canAttack = false;

        yield return new WaitForSeconds(attackDelay);

        if (!this.animator.GetCurrentAnimatorStateInfo(0).IsName("Attack 1"))
        {
            inputSource = 0;
            targetSpeed = 0;
        }

        attacking = false;
        canAttack = true;
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
}


