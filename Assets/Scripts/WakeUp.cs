using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WakeUp : MonoBehaviour
{
    PlayerController pc;
    Animator animator;
    public bool startAwake;
    bool awake;
    
    void Start()
    {
        pc = GetComponent<PlayerController>();
        animator = GetComponent<Animator>();

        if (startAwake == true){
            pc.enabled = enabled;
            awake = true;
            animator.SetBool("Awake", true);

        }
        else
        {
            pc.enabled = !enabled;
            awake = false;
        }
    }

    void Update()
    {
        if (Input.anyKeyDown && awake == false)
        {
            StartCoroutine("Wake");
        }
    }

    IEnumerator Wake()
    {
        awake = true;
        animator.SetBool("Wake", true);

        yield return new WaitForSeconds(4.5f);

        animator.SetBool("Awake", true);
        pc.enabled = enabled;
    }
}
