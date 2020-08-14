using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyInteract : MonoBehaviour
{
    
    public GameObject currentObject = null;
    bool held;
    bool hit;

    private void Start()
    {
        
    }

    void Update()
    {
        hit = this.GetComponent<EnemyMovement>().hit;
        
        //Pick Up
        if (currentObject)
        {
            currentObject.SendMessage("EnemyPickUp");
            held = true;
        }

        //Hit
        if (held == true && hit == true)
        {
            currentObject.SendMessage("EnemyDrop");
            currentObject = null;
            held = false;
        }
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Interactable") && held == false)
        {
            currentObject = other.gameObject;
        }
        if (other.gameObject.name == "Cauldron Trigger")
        {
            
        }
    }
}
