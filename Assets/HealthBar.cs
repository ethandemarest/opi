using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealthBar : MonoBehaviour
{
    public int maxHealth;
    public int currentHealth;

    void Start()
    {
        currentHealth = maxHealth;
    }

    void TakeDamage(int damage)
    {
        currentHealth = currentHealth-damage;
        if(currentHealth <= 0)
        {
            SendMessage("Death");
        }
    }

    void GainHealth(int bonusHealth)
    {
        currentHealth = currentHealth + bonusHealth;
    }
}
 