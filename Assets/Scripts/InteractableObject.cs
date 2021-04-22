using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InteractableObject : MonoBehaviour
{
    //ITEM
    Rigidbody2D rb;
    Animator itemAnim;
    BoxCollider2D itemCollider;
    Vector3 throwDirection;

    int itemStatus = 0;
    float throwDistance = 4;
    float smoothSpeed = 0.2f;
    public float speed;

    //OPI
    GameObject opi;
    GameObject opiCenter;
    GameObject itemHolder;

    public GameObject dust;

    //SHADOW
    public GameObject shadow;
    float targetOpac;
    Vector3 targetSize;
    static float t = 0.0f;
    bool shadowOn;


    void Start()
    {
        opi = GameObject.Find("Opi");
        opiCenter = GameObject.Find("Opi Center");
        itemHolder = GameObject.Find("Item Holder");
        rb = GetComponent<Rigidbody2D>();
        itemCollider = GetComponent<BoxCollider2D>();
        itemCollider.enabled = false;
        itemAnim = GetComponent<Animator>();

        StartCoroutine("Spawn");
    }

    void FixedUpdate()
    {

        if(itemStatus == 0) //O n Ground
        {
            transform.position = transform.position;
        }
        if (itemStatus == 1) // Walk
        {
            transform.position = itemHolder.transform.position;
        }
        if (itemStatus == 2) // Drop
        {
            rb.MovePosition(Vector3.Lerp(transform.position, throwDirection + new Vector3(0f,-0.6f,0f), smoothSpeed));
        }
        if(itemStatus == 3) // Up
        {
            rb.MovePosition(Vector3.Lerp(transform.position, itemHolder.transform.position, 0.4f));
        }
    }

    private void Update()
    {
        //Item Animation
        if (itemStatus == 1)
        {
            speed = opi.GetComponent<PlayerController>().movement.sqrMagnitude;
            itemAnim.SetFloat("Speed", speed);
        }
        else { speed = 0; }

        //Shadow Animation
        if (shadowOn)
        {
            t += 3f * Time.deltaTime;
            targetOpac = Mathf.Lerp(0, 1, t);
            targetSize = Vector3.Lerp(new Vector3(0f, 0f, 0f), new Vector3(0.2f, 0.2f, 0.2f),t);
            shadow.transform.GetComponent<SpriteRenderer>().material.color = new Color(1, 1, 1, targetOpac);
            shadow.transform.localScale = targetSize;
        }
        else
        {
            shadow.transform.localScale = new Vector3(0f,0f,0f);
            shadow.transform.GetComponent<SpriteRenderer>().material.color = new Color(1, 1, 1, 0);
        }
    }

    public void PickUp()
    {
        itemStatus = 1;
        shadowOn = false;
    }

    IEnumerator Spawn()
    {
        itemStatus = 3;
        yield return new WaitForSeconds(0.4f);
        itemCollider.enabled = true;
        itemStatus = 1;
    }

    IEnumerator Dropped(Vector2 throwDir)   
    {
        throwDirection.x = opiCenter.transform.position.x + throwDir.x * throwDistance;
        throwDirection.y = opiCenter.transform.position.y + throwDir.y * throwDistance;
        itemStatus = 2;

        itemAnim.SetBool("Item Drop", true);
        itemAnim.SetFloat("Speed", 0f);
        itemCollider.enabled = false;

        shadowOn = true;
        t = 0f;

        yield return new WaitForSeconds(0.2f);


        Instantiate(dust, transform.position + new Vector3(0f,-0.5f,0f), Quaternion.Euler(0f, 0f, 0f));

        yield return new WaitForSeconds(0.6f);

        itemStatus = 0;
        itemCollider.enabled = true;
    }

    IEnumerator AddIngredient()
    {
        itemAnim.SetBool("Submit", true);

        yield return new WaitForSeconds(1f);

        Destroy(gameObject);
    }
}
    