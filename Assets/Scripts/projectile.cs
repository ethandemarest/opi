using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class projectile : MonoBehaviour
{
    Rigidbody2D rb;
    public Vector3 velDir;
    public float maxSpeed;
    float speed;
    int frame;
    public int duration;
    GameObject opi;


    // Start is called before the first frame update
    void Start()
    {
        opi = GameObject.Find("Opi");
        rb = GetComponent<Rigidbody2D>();
        speed = 1f;
    }

    void Update()
    {
        frame++;
        speed++;

        if(speed >= maxSpeed)
        {
            speed = maxSpeed;
        }

        rb.velocity = -transform.up * speed/2;
        velDir = transform.InverseTransformDirection(rb.velocity);
        print(velDir);



        if (frame >= duration)
        {
            Destroy(gameObject);
        }
    }

}