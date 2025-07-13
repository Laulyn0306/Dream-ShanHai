using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class huoyanController : MonoBehaviour
{

    public float moveDistance = 2f;

    public float moveSpeed = 2f;

    private Vector3 startPos;

    public int damage = 10;
    // Start is called before the first frame update
    void Start()
    {
        startPos = transform.position;
    }

    // Update is called once per frame
    void Update()
    {
        float offset = Mathf.PingPong(Time.time * moveSpeed, moveDistance);
        transform.position = startPos + Vector3.up * offset;
    }
}
