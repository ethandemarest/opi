using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealthBar : MonoBehaviour
{
    public int maxHealth;
    public int currentHealth;
    bool dead;

    void Start()
    {
        dead = false;
        currentHealth = maxHealth;
    }

    private void Update()
    {
        if (currentHealth <= 0 && dead == false)
        {
            SendMessage("Death");
            dead = true;
        }
    }

    void TakeDamage(int damage)
    {
        currentHealth = currentHealth - damage;
    }

    void GainHealth(int bonusHealth)
    {
        currentHealth = currentHealth + bonusHealth;
    }
}
 