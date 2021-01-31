using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyMovemt2 : MonoBehaviour
{
    GameObject opi;
    GameObject opiCenter;
    GameObject enemCenter;

    GameObject enemyDetect;
    public GameObject nearbyEnemy = null;
    Vector2 otherEnemyPosition;
    public Vector2 enemDifference;

    public SpriteRenderer sprite;

    public float minDistance = 5.0f;
    public float movementSpeed = 100f;
    public float acceleration = 1f;
    private float curSpeed = 0.0f;

    public float knockBackPower = 3;
    public float knockDownTime = 0.6f;

    Vector2 knockBack;
    Vector2 opiLastMove;

    int focus;
    bool knockBackMovement;
    public bool invincible;


    // Update is called once per frame
    void Start()
    {
        sprite = GetComponent<SpriteRenderer>();
        focus = 1;
        knockBackMovement = false;

        opi = GameObject.Find("Opi");
        opiCenter = GameObject.Find("Opi Center");
        invincible = false;
        enemCenter = this.transform.GetChild(0).gameObject;


    }

    public void FixedUpdate()
    {
        enemyDetect = this.transform.GetChild(2).gameObject;
        otherEnemyPosition = enemyDetect.GetComponent<EnemyDetect>().otherEnemyPosition;
        nearbyEnemy = enemyDetect.GetComponent<EnemyDetect>().otherEnemy;

        //BEHAVIOR LIST
        Vector2[] targetPosition = new Vector2[2];
        targetPosition[0] = transform.position; //Enemy
        targetPosition[1] = opi.transform.position + new Vector3(0f, 1.8f, 0f); //Opi

        //SPEED LIST
        float[] targetSpeed = new float[2];
        targetSpeed[0] = 0f; //Enemy
        targetSpeed[1] = 1f; //Opi

        //Movement

        //DISTANCE BETWEEN ENEMY & OPI
        float opiDistance = Vector2.Distance(opiCenter.transform.position, (transform.position + new Vector3(0f, -1.2f, 0f)));

        if (knockBackMovement == true)
        {
            curSpeed -= acceleration*1.3f;
            transform.position = Vector2.MoveTowards(transform.position, knockBack, Mathf.Clamp(curSpeed,0f,500f) / 500);
        }
        else if (opiDistance >= minDistance)
        {
            curSpeed += acceleration;
            if (nearbyEnemy != null)
            {
                enemDifference.x = (transform.position.x - otherEnemyPosition.x);
                enemDifference.y = (transform.position.y - otherEnemyPosition.y);
            }
            else
            {
                enemDifference.x = (0f);
                enemDifference.y = (0f);
            }
            transform.position = Vector2.MoveTowards(transform.position, targetPosition[focus]+(enemDifference), curSpeed/1000);
        }

        if(opiDistance == minDistance)
        {
            curSpeed = 0f;
        }

        if (curSpeed > movementSpeed)
            curSpeed = movementSpeed;
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Attack") && invincible == false)
        {
            StopAllCoroutines();
            SendMessage("Hit");
            StartCoroutine("HitDelay");
            FindObjectOfType<AudioManager>().Play("Sword Hit");
        }

        if (other.CompareTag("Arrow"))
        {
            StopAllCoroutines();
            SendMessage("Hit");
            StartCoroutine("HitDelay");
            FindObjectOfType<AudioManager>().Play("Arrow Impact");
        }
        if (other.CompareTag("Opi")) //Runs After Succesful Attack to Opi
        {
            StopAllCoroutines();
            SendMessage("PostAttack");
            StartCoroutine("PostAttackDelay");
        }
    }

    public void Hit()
    {
        curSpeed = movementSpeed;

        opiLastMove.x = opi.GetComponent<PlayerController>().lastMove.x;
        opiLastMove.y = opi.GetComponent<PlayerController>().lastMove.y;

        knockBack.x = (this.transform.position.x + opiLastMove.x * knockBackPower);
        knockBack.y = (this.transform.position.y + opiLastMove.y * knockBackPower);
    }

    public void PostAttack()
    {
        curSpeed = movementSpeed;

        Vector2 difference = (enemCenter.transform.position - opiCenter.transform.position);

        knockBack.x = (enemCenter.transform.position.x + difference.normalized.x * knockBackPower);
        knockBack.y = (enemCenter.transform.position.y + difference.normalized.y * knockBackPower);
    }

    IEnumerator HitDelay()
    {
        //invincible = true;
        focus = 0; // Stop
        sprite.color = new Color(1, 1, 1, 0.5f);

        yield return new WaitForSeconds(0.15f);
        //invincible = false;
        sprite.color = new Color(1, 1, 1, 1);
        knockBackMovement = true;

        yield return new WaitForSeconds(knockDownTime);
        knockBackMovement = false;
        curSpeed = 0f;
        focus = 1; // Opi
    }
    IEnumerator PostAttackDelay()
    {
        focus = 0; // Stop

        yield return new WaitForSeconds(0.15f);
        knockBackMovement = true;

        yield return new WaitForSeconds(knockDownTime);
        knockBackMovement = false;
        curSpeed = 0f;
        focus = 1; // Opi
    }

}
