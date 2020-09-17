using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HealthBar : MonoBehaviour
{
    public Slider slider;
    public float hitTime;

    public float speed;
    public float time;

    float tempHealth;
    float currentHealth;

    public void SetMaxHealth(float health)
    {
        slider.maxValue = health;
        currentHealth = health; //10
        tempHealth = health; //10
    }

    public void SetHealth(float health)
    {
        tempHealth = health; //8
        StartCoroutine("DamageDelay");
    }
    IEnumerator DamageDelay()
    {
        yield return new WaitForSeconds(2f);
        currentHealth = tempHealth;
    }

    public void Update()
    {
        if (currentHealth != tempHealth)
        {
            time += Time.deltaTime * speed; //This will increment tParam based on Time.deltaTime multiplied by a speed multiplier
            slider.value = Mathf.Lerp(currentHealth, tempHealth, time);
        }
        else
        {
            time = 0;
        }


    }
}







