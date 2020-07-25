using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SlashControl : MonoBehaviour
{
    public Animator animator;
    public GameObject opi;
    private float lastMoveX;
    private float lastMoveY;

    // Update is called once per frame
    void Update()
    {



        lastMoveX = opi.GetComponent<PlayerController>().movement.x;
        lastMoveY = opi.GetComponent<PlayerController>().movement.y;

        if (lastMoveX > 0.01 || lastMoveX < -0.01 || lastMoveY > 0.01 || lastMoveY < -0.01)
        {
            animator.SetFloat("Last Move Horizontal", lastMoveX + lastMoveX);
            animator.SetFloat("Last Move Vertical", lastMoveY + lastMoveY);
        }

            


        //Animation
        if (opi.GetComponent<Animator>().GetCurrentAnimatorStateInfo(0).IsName("Attack 1"))
        {
            animator.SetBool("slash1", true);
            animator.SetBool("slash2", false);
        }
        if (opi.GetComponent<Animator>().GetCurrentAnimatorStateInfo(0).IsName("Attack 2"))
        {
            animator.SetBool("slash1", false);
            animator.SetBool("slash2", true);
        }
        if (!opi.GetComponent<Animator>().GetCurrentAnimatorStateInfo(0).IsName("Attack 1") && !opi.GetComponent<Animator>().GetCurrentAnimatorStateInfo(0).IsName("Attack 2"))
        {
            animator.SetBool("slash1", false);
            animator.SetBool("slash2", false);
        }

        
    }
}
    