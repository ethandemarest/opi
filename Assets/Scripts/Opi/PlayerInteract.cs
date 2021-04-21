using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using EZCameraShake;

public class PlayerInteract : MonoBehaviour
{
    CameraFollow cameraFollow;
    public GameObject currentObject = null;
    public GameObject ingredientHole = null;
    GameObject wizard;
    Animator animator;
    bool held;
    bool atCauldron;
    bool canPick;
    bool wasHit;
    bool interact, roll, attack, bow;
    public PlayerController playerController;

    private void Start()
    {
        cameraFollow = GameObject.Find("Camera Holder").GetComponent<CameraFollow>();
        wizard = GameObject.Find("Wizard");
        animator = GetComponent<Animator>();
        playerController = GetComponent<PlayerController>();
    }

    void Update()
    {
        wasHit = GetComponent<PlayerController>().wasHit;

        interact = Input.GetButtonDown("interact");
        roll = Input.GetButtonDown("roll");
        attack = Input.GetButtonDown("attack");
        bow = Input.GetButtonDown("bow");

        //Pick Up
        if (currentObject)  
        {
            PickUp();
            animator.SetBool("Item", true);
        }
        else
        {
            currentObject = null;
            animator.SetBool("Item", false);
        }

        //Add Ingredient
        if (currentObject && interact && atCauldron == true)
        {
            currentObject.SendMessage("AddIngredient"); //Ingredient Animation
            StartCoroutine("AddIngredient"); //Opi Animation 
            wizard.SendMessage("WizardReact"); //Wizard Animation

            currentObject = null;
            held = false;
            animator.SetBool("Item", false);
        }
    

        if(interact && canPick == true)
        {
            StartCoroutine("PickIngredient");
        }

        if(currentObject)
        {
            if (interact && atCauldron == false)
            {
                Drop();
            }
            if (attack || roll || bow || wasHit)
            {
                Drop();
            }
        }
    }

    void PickUp()
    {
        currentObject.SendMessage("PickUp");
    }
    void Drop()
    {
        currentObject.SendMessage("Dropped", playerController.lastMove);
        currentObject = null;
        held = false;
        animator.SetBool("Item", false);
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
        if (other.CompareTag("Ingredient"))
        {
            canPick = true;
            ingredientHole = other.gameObject;
        }
    }

    void OnTriggerExit2D(Collider2D other)
    {
        ingredientHole = null;
        canPick = false;
        if (other.gameObject.name == "Cauldron Trigger")
        {
            atCauldron = false;
        }
    }

    IEnumerator AddIngredient()
    {
        playerController.enabled = false;
        animator.SetBool("Scene Trigger", true);
        CameraShaker.Instance.ShakeOnce(0.5f, 10f, 1.8f, 0.1f);
        cameraFollow.CameraTrigger(new Vector3(0f, 15f, -50f), 6, 2f);

        yield return new WaitForSeconds(1f);

        CameraShaker.Instance.ShakeOnce(3f, 2f, 0.1f, 3f);
        cameraFollow.CameraTrigger(new Vector3(0f, 15f, -50f), 12, 0.2f);
        animator.SetBool("Item", false);

        yield return new WaitForSeconds(1f);

        cameraFollow.CameraTrigger(new Vector3(0f, 15f, -50f), 10, 1f);

        yield return new WaitForSeconds(1f);

        cameraFollow.CameraTrigger(new Vector3(0f, 15f, -50f), 10, 0.5f); //back to default
        playerController.enabled = true;
        currentObject = null;
    }


    IEnumerator PickIngredient()
    {
        transform.position = ingredientHole.transform.position + new Vector3(0f,-0.8f,0f);

        playerController.enabled = false;

        animator.SetFloat("Last Move Horizontal", 0f);
        animator.SetFloat("Last Move Vertical", -1f);

        ingredientHole.SendMessage("SpawnItem");

        animator.SetBool("Pick", true);
        animator.SetBool("Item", true);


        cameraFollow.angleNumber = 1;
        cameraFollow.CameraTrigger(new Vector3(transform.position.x, transform.position.y, -50f), 8, 0.2f);


        yield return new WaitForSeconds(1.6f);

        cameraFollow.angleNumber = 0;
        playerController.enabled = true;
    } 
}

