using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TexiaoController : MonoBehaviour
{
    public float magicHitDelay = 0.6f;
    
    private void OnTriggerEnter2D(Collider2D other)
    {
        Debug.Log("撞到了：" + other.name);
        if (other.CompareTag("Player"))
        {
            PlayerController player = other.GetComponent<PlayerController>();
            if (player != null)
            {
                player.TakeHit(magicHitDelay);
            }
        }
    }
}
