using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HealthBar : MonoBehaviour
{
    public GameObject gameObject;
    public Vector3 offset;

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

    void Update()
    {
        //this.transform.position = Camera.main.WorldToScreenPoint(parentTransform.position + offset);
        this.transform.position = Camera.main.WorldToScreenPoint(gameObject.transform.position + offset);

        if (currentHealth != tempHealth)
        {
            slider.value = tempHealth;
        }

        if (slider.value == 0)
        {
            print("destroyed");
            Destroy(this);
            Destroy(slider);
            Destroy(GetComponentInChildren<GameObject>());

        }


        /*
        if (currentHealth != tempHealth)
        {
            time += Time.deltaTime * speed; //This will increment tParam based on Time.deltaTime multiplied by a speed multiplier
            slider.value = Mathf.Lerp(currentHealth, tempHealth, time);
        }
        else
        {
            time = 0;
        }
        */

    }
}
