using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GrassBrush : MonoBehaviour
{
    public Animator animator;
    private bool brushed;


    private void OnTriggerEnter2D(Collider2D collision)
    {
        animator.SetBool("brushed", true);
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        animator.SetBool("brushed", false);
    }
}
