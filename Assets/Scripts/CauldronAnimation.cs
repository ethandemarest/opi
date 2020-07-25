using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CauldronAnimation : MonoBehaviour
{
    public Animator animator;

    // Update is called once per frame
    private void FixedUpdate()
    {
        if (GameObject.Find("Opi").GetComponent<Animator>().GetCurrentAnimatorStateInfo(0).IsName("ItemDrop"))
        {
            animator.SetBool("Item Input", true);
        }

        if (!GameObject.Find("Opi").GetComponent<Animator>().GetCurrentAnimatorStateInfo(0).IsName("ItemDrop"))
        {
            animator.SetBool("Item Input", false);
        }

    }

}