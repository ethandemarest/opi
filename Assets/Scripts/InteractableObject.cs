using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InteractableObject : MonoBehaviour
{
    //ITEM
    Rigidbody2D rb;
    Animator itemAnim;
    BoxCollider2D itemCollider;

    int itemStatus = 0;
    float throwDistance = 4;
    float smoothSpeed = 0.2f;
    float speed;

    //OPI
    GameObject opi;
    GameObject opiCenter;
    GameObject itemHolder;

    //SHADOW
    public GameObject shadow;

    Vector3 throwDirection;
    Vector3 pullDirection;


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
        shadow.SetActive(false);
    }

    void Update()
    {
        Vector3[] targetPosition = new Vector3[4];
        targetPosition[0] = transform.position;
        targetPosition[1] = itemHolder.transform.position;
        targetPosition[2] = throwDirection;

        if(itemStatus == 0)
        {
            transform.position = targetPosition[itemStatus];
        }
        if (itemStatus == 1)
        {
            transform.position = targetPosition[itemStatus];
        }
        if (itemStatus == 2)
        {
            rb.MovePosition(Vector3.Lerp(transform.position, targetPosition[itemStatus], smoothSpeed));
        }
        if(itemStatus == 3) // Up
        {
            rb.MovePosition(Vector3.Lerp(transform.position, itemHolder.transform.position, 0.4f));
        }
    }

    public void PickUp()
    {
        itemStatus = 1;
        shadow.SetActive(false);
    }

    IEnumerator Spawn()
    {
        pullDirection = transform.position;
        itemStatus = 3;
        yield return new WaitForSeconds(0.4f);
        itemCollider.enabled = true;
        itemStatus = 1;
    }

    IEnumerator Dropped(Vector2 throwDir)
    {
        itemCollider.enabled = false;
        itemAnim.SetBool("Item Drop", true);
        throwDirection.x = opiCenter.transform.position.x + throwDir.x * throwDistance;
        throwDirection.y = opiCenter.transform.position.y + throwDir.y * throwDistance;

        itemStatus = 2;

        print("throwDir = "+throwDir);
        yield return new WaitForSeconds(0.8f);

        itemCollider.enabled = true;
        shadow.SetActive(true);
        itemStatus = 0;
    }

    IEnumerator AddIngredient()
    {
        itemAnim.SetBool("Submit", true);

        yield return new WaitForSeconds(1f);

        Destroy(gameObject);
    }
}
    