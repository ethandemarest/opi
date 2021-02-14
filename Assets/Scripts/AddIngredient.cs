using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AddIngredient : MonoBehaviour
{
    GameObject currentObject = null;
    public InteractableObject currentObjScript;

    public bool atTrigger = false;


    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Opi"))
        {
            currentObject = other.gameObject;
            atTrigger = true;
        }
    }

    void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Opi"))
        {
            atTrigger = false;
        }
    }

}
    