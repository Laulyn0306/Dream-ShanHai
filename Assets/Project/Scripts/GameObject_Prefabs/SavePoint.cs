using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static PlayerDataSO;

public class SavePoint : MonoBehaviour
{
    public PlayerDataSO playerData;

    public bool oneTimeOnly = true;
    public SaveSlot slotToUse = 0;

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player") )
        {
            Debug.Log("玩家进入存档点");

            playerData.checkPoint = transform.position;
            playerData.SaveToSlot(slotToUse);

            
        }
    }
}
