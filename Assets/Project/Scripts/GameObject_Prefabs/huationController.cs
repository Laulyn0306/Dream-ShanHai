using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class huationController : MonoBehaviour
{


    public float moveDistance = 3f;
    public float moveSpeed = 2f;
    public bool moveHorizontally = true;

    private Vector3 startPos;
    private Vector3 targetPos;
    private bool movingToTarget=true;
    // Start is called before the first frame update
    void Start()
    {
        startPos = transform.position;
        targetPos = moveHorizontally ?
            startPos + Vector3.right * moveDistance :
            startPos + Vector3.up * moveDistance;
    }

    // Update is called once per frame
    void Update()
    {
        Vector3 destination = movingToTarget ? targetPos : startPos;
        transform.position = Vector3.MoveTowards(transform.position, destination, moveSpeed * Time.deltaTime);

        if (Vector3.Distance(transform.position, destination) < 0.01f)
        {
            movingToTarget = !movingToTarget;
        }
    }

    private void OnCollisionEnter2D(Collision2D other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            other.transform.SetParent(transform);
        }
    }

    private void OnCollisionExit2D(Collision2D other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            other.transform.SetParent(null);
        }
    }
}
