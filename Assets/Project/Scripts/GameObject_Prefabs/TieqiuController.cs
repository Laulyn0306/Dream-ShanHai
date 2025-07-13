using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TieqiuController : MonoBehaviour
{
   public float moveDistance = 2f;
   
    public float moveSpeed = 2f;

    private Vector3 startPos;


    public int damage = 10;

    private void Start()
    {
        startPos = transform.position;
    }

    private void Update()
    {
        float offset = Mathf.PingPong(Time.time * moveSpeed, moveDistance);
        transform.position = startPos + Vector3.up * offset;
    }

    
}
