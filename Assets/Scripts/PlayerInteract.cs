using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInteract : MonoBehaviour
{   
    public GameObject currentObject = null;
    Animator animator;
    bool held;
    bool atCauldron = false;
    

    private void Start()
    {
        held = false;
        
        animator = this.GetComponent<Animator>();
    }


    
    void Update()
    {
        //Pick Up
        if (currentObject)  
        {
            currentObject.SendMessage("DoInteraction1");
            held = true;
        }

        //Put Down
        if (Input.GetButtonDown("interact") && atCauldron == false && held == true)
        {
            currentObject.SendMessage("DoInteraction2");
            currentObject = null;
            held = false;
        }

        //Item Delivery
        if (Input.GetButtonDown("interact") && atCauldron == true && held == true)
        {
            currentObject.SendMessage("DoInteraction3");
            currentObject = null;
            held = false;
        }

        //Roll
        if (Input.GetButtonDown("roll") && held == true)
        {
            currentObject.SendMessage("DoInteraction2");
            currentObject = null;
            held = false;

        }

        //Attack
        if (Input.GetButtonDown("attack") && held == true)
        {
            currentObject.SendMessage("DoInteraction2");
            currentObject = null;
            held = false;
        }

    }

    
    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag ("Interactable") && held == false)
        {
            currentObject = other.gameObject;
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

