using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UI_A_Button : MonoBehaviour
{
    public Animator animator;

    void Update()
    {
        if (Input.GetButtonDown("attack"))
        {
            animator.SetBool("Down", true);
        }

        if (Input.GetButtonUp("attack"))
        {
            animator.SetBool("Up", true);
        }
    }
}
