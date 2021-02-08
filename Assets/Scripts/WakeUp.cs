using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WakeUp : MonoBehaviour
{
    //Camera camera;
    Animator animator;
    bool awake;
    
    // Start is called before the first frame update
    void Start()
    {
        //camera = Camera.main;
   
        awake = false;
        //camera.GetComponent<CameraFollow>().speed = 0;
        //camera.GetComponent<CameraFollow>().angle = 4;

        animator = this.GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.anyKeyDown && awake == false)
        {
            StartCoroutine("Delay");
            animator.SetBool("Wake", true);
        }

        if (this.animator.GetCurrentAnimatorStateInfo(0).IsName("Idle"))
        {
            animator.SetBool("Awake", true);
            this.GetComponent<PlayerController>().enabled = true;
            //camera.GetComponent<CameraFollow>().speed = 1;
            awake = true;
        }
            
    }

    IEnumerator Delay()
    {
        yield return new WaitForSeconds(0.1f);
        //camera.GetComponent<CameraFollow>().angle = 0;
    }
 


}
