using System.Collections;
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

    void Awake()
    {
        lastPos = this.transform.position;
    }

    private void FixedUpdate()
    {
        currentPos = this.transform.position;

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
                                                         