﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackController2 : MonoBehaviour
{
    Animator animator;
    public int noOfClicks = 0;
    float lastClickedTime = 0;
    public float maxComboDelay = 0.5f;
    public bool hasAttacked;

    // Start is called before the first frame update
    void Start()
    {
        animator = this.GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        if (Time.time - lastClickedTime > maxComboDelay)
        {
            noOfClicks = 0;
            animator.SetBool("Attack 1", false);
            animator.SetBool("Attack 2", false);

        }

        if (Input.GetButtonDown("attack"))
        {
            lastClickedTime = Time.time;
            noOfClicks++;



            if (noOfClicks % 2 == 1)
            {
                SendMessage("attackOne");
            }

            if (noOfClicks % 2 == 0)
            {
                SendMessage("attackTwo");
            }

        }
    }

    public void attackOne()
    {
        animator.SetBool("Attack 1", true);
    }

    public void attackTwo()
    {
        animator.SetBool("Attack 2", true);
    }

    private void LateUpdate()
    {
        if (this.animator.GetCurrentAnimatorStateInfo(0).IsName("Idle"))
        {
            hasAttacked = false;
        }
        animator.SetBool("Has Attacked", hasAttacked);


    }

}
