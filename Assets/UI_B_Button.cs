using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UI_B_Button : MonoBehaviour
{
    public Animator animator;

    void Update()
    {
        if (Input.GetButtonDown("bow"))
        {
            animator.SetBool("Down", true);
        }

        if (Input.GetButtonUp("bow"))
        {
            animator.SetBool("Up", true);
        }
    }
}
