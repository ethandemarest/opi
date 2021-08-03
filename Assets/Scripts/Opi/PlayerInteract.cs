using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using EZCameraShake;

public class PlayerInteract : MonoBehaviour
{
    CameraFollow cameraFollow;
    public GameObject currentObject = null;
    public GameObject ingredientHole = null;
    public GameObject cauldron = null;
    GameObject wizard;
    Animator animator;
    bool held;
    bool atCauldron;
    bool canPick;
    bool wasHit;
    public bool interact, roll, attack, bow;
    bool submitting;
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

        if (!submitting)
        {
            interact = Input.GetButtonDown("interact");
            roll = Input.GetButtonDown("roll");
            attack = Input.GetButtonDown("attack");
            bow = Input.GetButtonDown("bow");
        }else if (submitting)
        {
            interact = false;
            roll = false;
            attack = false;
            bow = false;
        }

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
        if(currentObject == null)
        {
            return;
        }
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
            cauldron = other.gameObject;
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
        submitting = true;
        transform.position = cauldron.transform.position + new Vector3(0f, -0.8f, 0f);
        playerController.enabled = false;
        animator.SetBool("Scene Trigger", true);

        CameraShaker.Instance.ShakeOnce(0.5f, 10f, 1.8f, 0.1f);
        cameraFollow.CameraTrigger(new Vector3(0f, 15f, -50f), 6, 2f);

        yield return new WaitForSeconds(1f);
        animator.SetFloat("Horizontal", 0f);
        animator.SetFloat("Vertical", 0f);
        animator.SetFloat("Last Move Horizontal", 0f);
        animator.SetFloat("Last Move Vertical", 1f);
        animator.SetFloat("Speed", 0f);
        CameraShaker.Instance.ShakeOnce(3f, 2f, 0.1f, 3f);
        cameraFollow.CameraTrigger(new Vector3(0f, 15f, -50f), 12, 0.2f);
        animator.SetBool("Item", false);

        yield return new WaitForSeconds(1f);

        cameraFollow.CameraTrigger(new Vector3(0f, 15f, -50f), 10, 1f);

        yield return new WaitForSeconds(1f);

        cameraFollow.CameraTrigger(new Vector3(0f, 15f, -50f), 10, 0.5f); //back to default
        playerController.enabled = true;
        currentObject = null;
        submitting = false;
    }


    IEnumerator PickIngredient()
    {
        if (ingredientHole.GetComponent<SpawnIngredient>().picked)
        {
            yield break;
        }

        submitting = true;

        transform.position = ingredientHole.transform.position + new Vector3(0f,-0.3f,0f);

        playerController.enabled = false;

        animator.SetFloat("Last Move Horizontal", 0f);
        animator.SetFloat("Last Move Vertical", -1f);

        ingredientHole.SendMessage("SpawnItem");

        animator.SetBool("Pick", true);

        cameraFollow.angleNumber = 1;
        cameraFollow.CameraTrigger(new Vector3(transform.position.x, transform.position.y, -50f), 8, 0.2f);


        yield return new WaitForSeconds(1.6f);

        cameraFollow.angleNumber = 0;
        playerController.enabled = true;
        submitting = false;
    }
}

