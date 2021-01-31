using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySlash : MonoBehaviour
{
    Animator animator;

    private void Start()
    {
        animator = this.GetComponent<Animator>();
    }


    void Update()
    {
        if (this.animator.GetCurrentAnimatorStateInfo(0).IsName("Empty"))
        {
            Destroy(this.gameObject);
        }
        
    }

}
