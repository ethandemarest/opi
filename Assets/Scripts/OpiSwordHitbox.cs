using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OpiSwordHitbox: MonoBehaviour
{
    GameObject opi;
    public GameObject currentObject = null;

    SpriteRenderer sp;
    PolygonCollider2D swordCollider;
    PlayerController playerController;


    Vector2 swingDirection;
    public Vector3 offset;
    float angle;

    int frameCount;
    public int duration;

    // Start is called before the first frame update
    void Start()
    {
        sp = GetComponent<SpriteRenderer>();
        frameCount = 0;
        swordCollider = GetComponent<PolygonCollider2D>();

        opi = GameObject.Find("Opi");
        playerController = opi.GetComponent<PlayerController>();
    }

    // Update is called once per frame
    void Update()
    {
        frameCount++;
        if (frameCount >= duration)
        {
            //sp.enabled = false;
            swordCollider.enabled = false;
        }

        transform.position = opi.transform.position + offset;


        transform.rotation = Quaternion.Euler(0f, 0f, angle);

        //Aim Calculation
        swingDirection.x = playerController.lastMove.x;
        swingDirection.y = playerController.lastMove.y;
        angle = Mathf.Atan2(swingDirection.y, swingDirection.x) * Mathf.Rad2Deg;
    }

    public void Attack()
    {
        frameCount = 0;
        //sp.enabled = true;
        swordCollider.enabled = true;
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Enemy"))
        {
            currentObject = other.gameObject;
        }
        else
        {
            currentObject = null;
        }
    }
}
    