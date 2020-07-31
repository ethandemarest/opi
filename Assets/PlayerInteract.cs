using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInteract : MonoBehaviour
{   
    GameObject currentObject = null;
    public InteractableObject currentObjScript;
    bool held;
    bool atCauldron = false;

    void Update()
    {

        Debug.Log(atCauldron);
        //Pick Up
        if (currentObject && held == false)  
        {
            if(held == false)
            {
                currentObject.SendMessage("DoInteraction1");
            }
        }

        //Put Down
        if (Input.GetButtonDown("interact") && atCauldron == false)
        {
            if (currentObject)
            {
                currentObject.SendMessage("DoInteraction2");
                currentObject = null;
            }
        }

        //Item Delivery
        if (Input.GetButtonDown("interact") && atCauldron == true)
        {
            // THIS IS WHERE WE ADD "SEND MESSAGE 3" BUT NEED TO ADD INVENTORY SYSTEM TO REMEMBER CURRENT OBJECT
        }

    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag ("Interactable"))
        {
            currentObject = other.gameObject;
            held = currentObject.GetComponent<InteractableObject>().held;
        }
        if (other.gameObject.name == "Cauldron Trigger")
        {
            atCauldron = true;
        }

        
    }

    void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Interactable"))
        {

        }

        if (other.gameObject.name == "Cauldron Trigger")
        {
            atCauldron = false;
        }
    }

  

}
