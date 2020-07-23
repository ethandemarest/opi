using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemDropTrigger : MonoBehaviour
{
    public Animator animator;
    public bool triggerCheck;
    public Vector3 witchCamera;

    //Enter Trigger
    public void OnTriggerEnter2D(Collider2D collision)
    {
        triggerCheck = true;
    }


    public void OnTriggerExit2D(Collider2D collision)
    {
        triggerCheck = false;
    }
  
}
