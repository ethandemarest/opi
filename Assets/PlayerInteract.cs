using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInteract : MonoBehaviour
{
    int noOfClicks = 0;

    GameObject currentObject = null;

    void Update()
    {
        //Put Down
        if (Input.GetButtonDown("interact") && noOfClicks > 0)
        {
            currentObject.SendMessage("DoInteraction2");

        }

        //Pick Up
        if (Input.GetButtonDown("interact") && currentObject && noOfClicks == 0)
        {
            currentObject.SendMessage ("DoInteraction1");
            
        }
    }


    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag ("Interactable"))
        {
            currentObject = other.gameObject;
            noOfClicks = 0;
        }
    }

    void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Interactable"))
        {
            if (other.gameObject == currentObject)
            {
                currentObject = null;
                noOfClicks = 1;
            }
        }
    }

  

}
