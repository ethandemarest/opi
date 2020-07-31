using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInteract : MonoBehaviour
{   

    GameObject currentObject = null;
    public InteractableObject currentObjScript;
    bool held;

    private void Start()
    {
        
    }

    void Update()
    {
        

        //Pick Up
        if (currentObject && held == false)  
        {

            if(held == false)
            {
                print("held is false");

                currentObject.SendMessage("DoInteraction1");
            }
            
        }

        //Put Down
        if (Input.GetButtonDown("interact"))
        {
            if (currentObject)
            {
                currentObject.SendMessage("DoInteraction2");
                currentObject = null;
            }
            
        }
    }


    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag ("Interactable"))
        {
            currentObject = other.gameObject;
            held = currentObject.GetComponent<InteractableObject>().held;
        }
    }

    void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Interactable"))
        {
            if (other.gameObject == currentObject)
            {
                
            }
        }
    }

  

}
