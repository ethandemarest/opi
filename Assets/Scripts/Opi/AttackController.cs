using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackController : MonoBehaviour
{
    public Animator animator;
    public int noOfClicks = 0;
    float lastClickedTime = 0;
    public float maxComboDelay = 0.9f;
    public bool hasAttacked;

    // Start is called before the first frame update
    void Start()
    {
        animator = gameObject.GetComponent<Animator>();
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
            animator.SetInteger("Hits 1", noOfClicks);
            hasAttacked = true;
            
           

            if (noOfClicks % 2 == 1)
            {
                

                
                animator.SetBool("Attack 2", false);
                animator.SetBool("Attack 1", true);

                
            }

            if (noOfClicks % 2 == 0)
            {
                

                
                animator.SetBool("Attack 1", false);
                animator.SetBool("Attack 2", true);
            }

        }
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
