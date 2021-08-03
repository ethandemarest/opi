using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class projectile : MonoBehaviour
{
    Animator animator;
    Rigidbody2D rb;

    public Vector3 velDir;
    public Vector2 reflect;

    public float maxSpeed;
    public float duration;
    float currentSpeed;
    float angle;

    int frame;
    bool reflected;



    GameObject opi;
    public GameObject projectileDust;
    public GameObject impact;
    public GameObject reflectEffect;

    private void Awake()
    {
        StartCoroutine("Destroy");
    }

    void Start()
    {
        animator = GetComponent<Animator>();
        rb = GetComponent<Rigidbody2D>();
        opi = GameObject.Find("Opi");

        currentSpeed = 1f;
        reflected = false;
    }

    void Update()
    {
        frame++;
        currentSpeed++;

        if(currentSpeed >= maxSpeed){
            currentSpeed = maxSpeed;
        }

        if(reflected == true){
            rb.velocity = reflect * currentSpeed / 2;
        }
        else{
            rb.velocity = transform.right * currentSpeed / 2;
        }

        velDir = transform.InverseTransformDirection(rb.velocity);

        if (frame >= 2) {
            Instantiate(projectileDust, transform.position, Quaternion.Euler(0f, 0f, Random.Range(0, 360)));
            frame = 0;
        }

    }

    IEnumerator Destroy()
    {
        yield return new WaitForSecondsRealtime(duration);
        animator.SetBool("End", true);
        yield return new WaitForSecondsRealtime(1f);
        Destroy(gameObject);
    }


    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("OpiDamage"))
        {
            StopAllCoroutines();

            transform.gameObject.tag = "EnemySpellReflected";
            angle = Mathf.Atan2(opi.GetComponent<PlayerController>().lastMove.y, opi.GetComponent<PlayerController>().lastMove.x) * Mathf.Rad2Deg;
            transform.rotation = Quaternion.Euler(0f, 0f, angle);

            reflected = true;
            reflect = opi.GetComponent<PlayerController>().lastMove.normalized;

            Instantiate(reflectEffect, transform.position, Quaternion.Euler(0f, 0f, angle));

            FindObjectOfType<AudioManager>().Play("Sword Hit");
            FindObjectOfType<AudioManager>().Play("Spellcaster Deflect");
            FindObjectOfType<AudioManager>().Play("Sword Hit");

            animator.SetBool("Reflected", true);
            StartCoroutine("Destroy");
        }


        if (other.CompareTag("Opi") && !opi.GetComponent<PlayerController>().rolling)
        {
            Instantiate(impact, transform.position, Quaternion.Euler(0f, 0f, 0f));
            Destroy(gameObject);
        }

        if (other.CompareTag("Enemy") && reflected == true)
        {
            Instantiate(impact, transform.position, Quaternion.Euler(0f, 0f, 0f));
            Destroy(gameObject);
        }
    }
}