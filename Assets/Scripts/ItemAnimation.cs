using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemAnimation : MonoBehaviour
{
    private Animator animator;
    private Rigidbody2D rb;
    private GameObject item;
    private GameObject opi;
    private Vector3 opiPosition;

    public bool sceneTrigger;
    public Vector3 offset;

    public bool hasDropped;



    // Start is called before the first frame update
    void Start()
    {
        item = gameObject;
        opi = GameObject.Find("Opi");
        animator = item.GetComponent<Animator>();
        rb = item.GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void Update()
    {
        opiPosition = opi.GetComponent<Transform>().position;
        animator = item.GetComponent<Animator>();

        // Bounce
        if (opi.GetComponent<Animator>().GetCurrentAnimatorStateInfo(0).IsName("Holding Movement"))
        {
            animator.SetBool("Opi Walking", true);
        }
        else
        {
            animator.SetBool("Opi Walking", false);
        }

      


        //Item Submit
        animator.SetBool("Submit", sceneTrigger);
        
        //Destroy
        if (animator.GetCurrentAnimatorStateInfo(0).IsName("Item Empty"))
        {
            Destroy(item);
        }
    }

    
       


}   
