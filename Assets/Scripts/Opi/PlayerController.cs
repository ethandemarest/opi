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
    
    SpriteRenderer sprite;

    //Movement
    Rigidbody2D rb;
    Animator animator;

    [Header("||Movement||")]
    public float speed = 10f;
    [HideInInspector]
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
    public float rollRecharge;
    bool invincible;
    bool bounce;

    [HideInInspector]
    public bool rolling;
    int inputSource;
    public bool roll;
    public bool canRoll;

    //Interact
    [HideInInspector]
    public bool sceneTrigger, interact, canInteract, hit;
    [HideInInspector]
    public bool wasHit;
    bool atCauldron;    

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
        atCauldron = false;
        canInteract = true;
        canRoll = true;
        attacking = false;
        canAttack = true;
        wasHit = false; 
        inputSource = 0;
        lastMove.y = -1f;
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
            //BOW CONTROLS
            if (Input.GetButtonDown("bow")){  
                draw = true;}
            if (Input.GetButtonUp("bow")){
                draw = false;}
            interact = Input.GetButtonDown("interact"); //ITEM CONTROLS
        }
        else
        {
            movement.x = lastMove.x;
            movement.y = lastMove.y;
            roll = false;
            attack = false;
            draw = false;
            interact = false;
            speed = 0f;
        }

        //Roll

        if (roll && canRoll == true && bowReady == true)
        {
            animator.SetBool("Roll", roll);
            StartCoroutine("Rolling", lastMove);
        }

        //Attack
        if (Time.time - lastClickedTime > maxComboDelay) { noOfClicks = 0; }

        if (attack
            && rolling == false
            && canAttack == true
            && wasHit == false)
        {
            lastClickedTime = Time.time;
            noOfClicks++;
            if (noOfClicks % 2 == 1)
            {
                StopAllCoroutines();
                hitbox.SendMessage("Attack");
                StartCoroutine("AttackOne", lastMove);
                animator.SetBool("Attack 1", true);
            }
            if (noOfClicks % 2 == 0)
            {
                StopAllCoroutines();
                hitbox.SendMessage("Attack");
                StartCoroutine("AttackTwo", lastMove);
                animator.SetBool("Attack 2", true);
            }
        }

        if (rolling == false)
        {
            if (draw && bowReady == true){
                StartCoroutine("BowDraw"); }
            if (!draw && arrowReady){
                StartCoroutine("BowShoot");}
        }

        if (interact && canInteract == true && atCauldron == true)
        {
            StartCoroutine("Interact");
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
        //Movement Expression
        if (attacking == true)
        {
            transform.position = Vector2.Lerp(transform.position, knockBack, 0.05f);
        }
        if (rolling == true)
        {
            rb.MovePosition(rb.position + lockDirection * rollSpeed * Time.fixedDeltaTime);
        }
        if (wasHit == true)
        {
            rb.position = Vector2.Lerp(transform.position, knockBack, 0.05f);
        }
        else if (rolling == false && bowReady == true && attacking == false)
        {
            rb.MovePosition(rb.position + movement.normalized * speed * Time.fixedDeltaTime);
            animator.SetFloat("Speed", (movement.sqrMagnitude * speed));
        }
        else
        {
            animator.SetFloat("Speed", (0));
        }
    }

    //TRIGGERS
    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Scene")){
            atCauldron = true;
        }
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
    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Scene")){
            atCauldron = false;
        }
    }
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if(rolling == true && collision.gameObject.CompareTag("Environment")){
            print(collision.gameObject.name);
            StartCoroutine("Bounce", lockDirection);
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

    IEnumerator AttackOne(Vector2 attackDir)
    {
        SendMessage("SlashOne");

        string[] opiSound = new string[3];
        opiSound[0] = ("Opi Voice Swing 1");
        opiSound[1] = ("Opi Voice Swing 2");
        opiSound[2] = ("Opi Voice Swing 3");
        FindObjectOfType<AudioManager>().Play(opiSound[Random.Range(0, 3)]);
        FindObjectOfType<AudioManager>().Play("Sword Swing");

        canAttack = false;
        attacking = true;
        lockDirection = attackDir;

        knockBack.x = transform.position.x + attackDir.x * swingForce;
        knockBack.y = transform.position.y + attackDir.y * swingForce;

        inputSource = 1;

        yield return new WaitForSeconds(attackDelay);

        canAttack = true;

        yield return new WaitForSeconds(attackDelay);

        canRoll = true;
        attacking = false;
        inputSource = 0;
    }

    IEnumerator AttackTwo(Vector2 attackDir)
    {
        SendMessage("SlashTwo");

        string[] opiSound = new string[3];
        opiSound[0] = ("Opi Voice Swing 1");
        opiSound[1] = ("Opi Voice Swing 2");
        opiSound[2] = ("Opi Voice Swing 3");
        FindObjectOfType<AudioManager>().Play(opiSound[Random.Range(0, 3)]);
        FindObjectOfType<AudioManager>().Play("Sword Swing");

        canAttack = false;
        attacking = true;
        lockDirection = attackDir;

        knockBack.x = transform.position.x + attackDir.x * swingForce;
        knockBack.y = transform.position.y + attackDir.y * swingForce;

        inputSource = 1;

        yield return new WaitForSeconds(attackDelay);

        canAttack = true;

        yield return new WaitForSeconds(attackDelay);

        canRoll = true;
        attacking = false;
        inputSource = 0;
    }

    IEnumerator Interact()
    {
        playerController = false;
        canInteract = false;

        CameraShaker.Instance.ShakeOnce(0.5f, 10f, 1.8f, 0.1f);
        cameraFollow.CameraTrigger(new Vector3(0f,15f,-50f), 6, 2f);

        yield return new WaitForSeconds(1f);

        CameraShaker.Instance.ShakeOnce(3f, 2f, 0.1f, 3f);
        cameraFollow.CameraTrigger(new Vector3(0f, 15f, -50f), 12, 0.2f);

        yield return new WaitForSeconds(1f);

        cameraFollow.CameraTrigger(new Vector3(0f, 15f, -50f), 10, 1f);

        yield return new WaitForSeconds(1f);

        cameraFollow.CameraTrigger(new Vector3(0f, 15f, -50f), 10, 0.5f); //back to default

        canInteract = true;
        playerController = true;

    }

    IEnumerator Rolling(Vector2 rollDir)
    {
        gameObject.layer = 9;
        lockDirection = lastMove.normalized;

        invincible = true;
        rolling = true;
        canRoll = false;
        lockDirection = lastMove.normalized;
        inputSource = 1;

        yield return new WaitForSeconds(rollDuration);

        gameObject.layer = 8;
        invincible = false;
        rolling = false;
        inputSource = 0;

        yield return new WaitForSeconds(rollRecharge);

        canRoll = true;
    }   

    IEnumerator BowDraw()
    {
        bow.SendMessage("BowDraw");
        bowReady = false;
        attacking = false;

        animator.SetBool("Bow", true);
        FindObjectOfType<AudioManager>().Play("Bow Draw");


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

        yield return new WaitForSeconds(0.01f);

        animator.SetBool("Shoot", false);

        yield return new WaitForSeconds(0.5f);

        animator.SetBool("Shoot", false);
        inputSource = 0;
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

        yield return new WaitForSeconds(knockDownTime);

        invincible = false;
        sprite.color = new Color(1, 1, 1, 1);
        wasHit = false;
        canRoll = true;
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

 
        difference.x = (transform.position.x - bounceDir.x);
        difference.y = (transform.position.y - bounceDir.y);


        knockBack.x = transform.position.x - difference.normalized.x * damageKnockBack;
        knockBack.y = transform.position.y - difference.normalized.y * damageKnockBack;

        wasHit = true;
        canRoll = false;

        yield return new WaitForSeconds(knockDownTime);

        invincible = false;
        sprite.color = new Color(1, 1, 1, 1);
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
    



