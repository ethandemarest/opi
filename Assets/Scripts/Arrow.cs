﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Arrow : MonoBehaviour
{
    public SpriteRenderer spriteRenderer;
    public Rigidbody2D rb;
    public Animator animator;
    public float speed = 20f;

    Vector3 lastPos;
    Vector3 currentPos;
    public Vector3 shootDir;

    void Awake()
    {
        shootDir = GameObject.Find("Opi").GetComponent<PlayerController>().lastMove;
        lastPos = transform.position;
    }

    private void FixedUpdate()
    {
        currentPos = transform.position;

        if (currentPos.x < lastPos.x)
        {
            spriteRenderer.flipX = true;
        }
    }


    void OnTriggerEnter2D(Collider2D other)
    {
        //Environment Impact
        if (other.CompareTag("Environment"))
        {
            rb.velocity = new Vector3(0, 0, 0);
            animator.SetBool("Impact", true);
            rb.bodyType = RigidbodyType2D.Static;
        }

        if (other.CompareTag("Enemy"))
        {
            Destroy(gameObject);
        }
    }
}
                                                          