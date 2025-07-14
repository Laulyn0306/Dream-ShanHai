using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class EnemyHealth : MonoBehaviour
{
    public int maxHealth = 100;
    public int currentHealth;

    public UnityEvent onDeath;
    public UnityEvent onHit;
    void Start()
    {
        currentHealth = maxHealth;
    }

   public void TakeDamage(int amount)
   {
        currentHealth -= amount;
        currentHealth = Mathf.Clamp(currentHealth, 0, maxHealth);

        onHit?.Invoke();

        if (currentHealth <= 0)
        {
            Die();
        }
   }

    public void Heal(int amount)
    {
        currentHealth += amount;
        currentHealth = Mathf.Clamp(currentHealth, 0, maxHealth);
    }


    void Die()
    {
        Debug.Log($"{gameObject.name} 死了");
        onDeath?.Invoke();
         Destroy(gameObject);
    }
}
