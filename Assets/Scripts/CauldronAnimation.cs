using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CauldronAnimation : MonoBehaviour
{
    public Animator animator;

    // Update is called once per frame
    private void FixedUpdate()
    {
        bool opiInteract = GameObject.Find("Opi").GetComponent<PlayerInteract>().sceneTrigger;

        animator.SetBool("Item Input", opiInteract);

    }

}