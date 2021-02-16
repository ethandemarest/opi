﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInteract : MonoBehaviour
{   
    public GameObject currentObject = null;
    bool held;
    bool atCauldron = false;
    bool wasHit;
    public PlayerController playerController;

    private void Start()
    {
        playerController = this.GetComponent<PlayerController>();
        held = false;
    }

    void Update()
    {
        wasHit = this.GetComponent<PlayerController>().wasHit;

        //Pick Up
        if (currentObject)  
        {
            currentObject.SendMessage("OpiPickUp");
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
            playerController.StartCoroutine("Interact");
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

        //Bow
        if (Input.GetButtonDown("bow") && held == true)
        {
            currentObject.SendMessage("DoInteraction2");
            currentObject = null;
            held = false;
        }

        //Hit
        if (wasHit == true && held == true)
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
