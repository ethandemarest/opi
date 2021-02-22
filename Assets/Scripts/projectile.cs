using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class projectile : MonoBehaviour
{
    Rigidbody2D rb;
    public Vector3 velDir;
    Vector2 reflect;
    public float maxSpeed;
    float speed;
    int frame;
    int frame2;
    public int duration;
    public GameObject projectileDust;
    public GameObject impact;
    public GameObject reflectEffect;
    GameObject opi;
    bool reflected;

    float angle;


    // Start is called before the first frame update
    void Start()
    {
        opi = GameObject.Find("Opi");
        rb = GetComponent<Rigidbody2D>();
        speed = 1f;
        reflected = false;
    }

    void Update()
    {
        frame++;
        frame2++;
        speed++;

        if(speed >= maxSpeed)
        {
            speed = maxSpeed;
        }

        if(reflected == true)
        {
            rb.velocity = reflect * speed / 2;
        }
        else
        {
            rb.velocity = transform.right * speed / 2;

        }
        velDir = transform.InverseTransformDirection(rb.velocity);



        if (frame2 >= 2)
        {
            Instantiate(projectileDust, transform.position, Quaternion.Euler(0f, 0f, Random.Range(0, 360)));
            frame2 = 0;
        }






        if (frame >= duration)
        {
            Destroy(gameObject);
        }
    }






    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("OpiDamage"))
        {
            FindObjectOfType<AudioManager>().Play("Spellcaster Deflect");
            transform.gameObject.tag = "Arrow";
            angle = Mathf.Atan2(opi.GetComponent<PlayerController>().lastMove.y, opi.GetComponent<PlayerController>().lastMove.x) * Mathf.Rad2Deg;
            transform.rotation = Quaternion.Euler(0f, 0f, angle);
            Instantiate(reflectEffect, transform.position, Quaternion.Euler(0f, 0f, angle));


            FindObjectOfType<AudioManager>().Play("Sword Hit");
            reflected = true;
            frame = 0;
            reflect = opi.GetComponent<PlayerController>().lastMove.normalized;
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