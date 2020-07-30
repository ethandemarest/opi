using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInteract : MonoBehaviour
{   

    GameObject currentObject = null;
    public InteractableObject currentObjScript;
    bool held;

    void Update()
    {
        

        //Pick Up
        if (currentObject)  
        {

            currentObject.SendMessage("DoInteraction1");

        }

        //Put Down
        if (Input.GetButtonDown("interact") && held == true)
        {
            currentObject.SendMessage("DoInteraction2");
            currentObject = null;

        }
    }


    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag ("Interactable"))
        {
            currentObject = other.gameObject;            
        }
    }

    void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Interactable"))
        {
            if (other.gameObject == currentObject)
            {
                held = true;
                
            }
        }
    }

  

}
