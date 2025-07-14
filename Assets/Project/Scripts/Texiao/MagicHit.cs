using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MagicHit : MonoBehaviour
{

    public float damageAmount = 1f;
    public float hitDelay = 0.5f;


    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            Debug.Log("魔法击中玩家！");

            PlayerController player = collision.GetComponent<PlayerController>();
            if (player != null)
            {
                player.healthUI.TakeDamage(damageAmount);
                player.PlayHurtAnimation();
                player.RestroeControlAfterDelay(hitDelay);
                
            }
        }
    }
}
