using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WizardAnimation : MonoBehaviour
{
    public Animator animator;

    void WizardReact()
    {
        animator.SetBool("Reaction", true);
    }
}
