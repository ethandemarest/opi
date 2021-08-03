using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIBar : MonoBehaviour
{
    public bool followParent;
    public GameObject parent;
    public Vector3 offset;

    public Slider sliderGreen;
    public Slider sliderYellow;

    public GameObject effect;
    public float maxStamina;
    public float currentStamina;
    public float recoverySpeed;
    public float regenDelay;
    public float differenceDelay;

    float maxDelay;
    float currentDelay;

    bool regen;
    bool diffMove;

    void Start()
    {
        currentStamina = maxStamina;
        currentDelay = maxStamina;
    }

    void FixedUpdate()
    {
        if (followParent)
        {
            transform.position = parent.transform.position + offset * Time.deltaTime;
        }

        if (regen == true)
        {
            if (currentStamina < maxStamina)
            {
                currentStamina += recoverySpeed * Time.deltaTime;
            }
            else
            {
                currentStamina = maxStamina;
            }

            currentDelay = currentStamina;
        }

        if (diffMove == true)
        {
            if (currentDelay > currentStamina)
            {
                currentDelay -= 10f * Time.deltaTime;
            }
        }

        float realDif = Mathf.Clamp(currentDelay, 0f, maxStamina);

        sliderGreen.value = currentStamina;
        sliderYellow.value = realDif;
    }

    void UseEnergy(float energyUsed)
    {
        StopAllCoroutines();
        //Instantiate(effect, transform.position, Quaternion.Euler(0f, 0f, 0f));

        regen = false;
        currentStamina -= energyUsed;

        if (currentStamina <= 0)
        {
            currentStamina = 0;
        }

        diffMove = false;
        StartCoroutine("StaminaDelay");
        StartCoroutine("DelayDelay");


    }
    IEnumerator StaminaDelay()
    {
        yield return new WaitForSecondsRealtime(regenDelay);
        regen = true;
    }
    IEnumerator DelayDelay()
    {
        yield return new WaitForSecondsRealtime(differenceDelay);
        diffMove = true;
    }
}
