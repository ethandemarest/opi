using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIBarHealth : MonoBehaviour
{
    public bool followParent;
    public GameObject parent;
    public GameObject[] healthBarElements;

    public Vector3 offset;

    public Slider sliderGreen;
    public Slider sliderYellow;

    public float maxHealth;
    float currentStamina;
    float differenceDelay = 0.5f;

    float currentDelay;

    bool diffMove;
    bool dead;

    void Start()
    {
        maxHealth = parent.GetComponent<HealthBar>().maxHealth;
        sliderGreen.maxValue = maxHealth;
        sliderYellow.maxValue = maxHealth;

        currentStamina = maxHealth;
        currentDelay = maxHealth;
    }   

    void FixedUpdate()
    {
        if (followParent)
        {
            transform.position = parent.transform.position + offset * Time.deltaTime;
        }

        if (diffMove == true)
        {
            if (currentDelay > currentStamina)
            {
                currentDelay -= 20f * Time.deltaTime;
            }
        }

        float realDif = Mathf.Clamp(currentDelay, 0f, maxHealth);

        sliderGreen.value = currentStamina;
        sliderYellow.value = realDif;

        if(currentStamina <= 0 && !dead)
        {
            dead = true;
            parent.SendMessage("Death");
        }
    }

    void UseEnergy(float energyUsed)
    {
        StopAllCoroutines();
        StartCoroutine("TransparencyDelay");

        currentStamina -= energyUsed;

        if (currentStamina <= 0)
        {
            currentStamina = 0;
        }

        diffMove = false;
        StartCoroutine("DelayDelay");
    }

    IEnumerator DelayDelay()
    {
        yield return new WaitForSecondsRealtime(differenceDelay);
        diffMove = true;
    }

    IEnumerator TransparencyDelay()
    {
        foreach (GameObject e in healthBarElements)
        {
            e.SetActive(true);
        }

        yield return new WaitForSecondsRealtime(2f);

        foreach (GameObject e in healthBarElements)
        {
            e.SetActive(false);
        }
    }
}
