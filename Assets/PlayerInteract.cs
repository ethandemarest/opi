﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInteract : MonoBehaviour
{
    public Animator animator;
    public bool sceneTrigger;
    public bool interact;
    public bool cameraTrigger;


    // Update is called once per frame
    void Update()
    {
        //Trigger Check
        bool triggerCheck = GameObject.Find("ItemDropTrigger").GetComponent<ItemDropTrigger>().triggerCheck;
        bool interact = GameObject.Find("Opi").GetComponent<PlayerController>().itemDrop;

        Debug.Log(interact);

        animator.SetBool("Cauldron", triggerCheck);

        //Input
        if (interact == true && triggerCheck == true)
        {
            animator.SetBool("OpiItemPlace", interact);
        }
        else
        {
            animator.SetBool("OpiItemPlace", false);
        }


        //Animation
        if (this.animator.GetCurrentAnimatorStateInfo(0).IsName("ItemDrop"))
        {
            sceneTrigger = true;
        }
        else
        {
            sceneTrigger = false;
        }

    }

    //Triggers
    public GameObject TestTrigger;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        cameraTrigger = true;
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        cameraTrigger = false;
    }
    
}
