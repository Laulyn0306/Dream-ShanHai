using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public float movespeed = 5f;
    public float jumpForce = 5f;
    private Rigidbody2D rb;
    private bool isGrounded;

    public Transform groundCheck;
    public float checkRadius = 0.2f;
    public LayerMask groundLayer;

    public Animator player;
    private float lastMoveDirection = 1f;
    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void Update()
    {
        float moveInput = Input.GetAxisRaw("Horizontal");
        
        rb.velocity = new Vector2(moveInput * movespeed, rb.velocity.y);

        isGrounded = Physics2D.OverlapCircle(groundCheck.position, checkRadius, groundLayer);

        if (Input.GetKeyDown(KeyCode.Space) && isGrounded)
        {
            rb.velocity = new Vector2(rb.velocity.x, jumpForce);
        }


        
        PlayCharacterAni();
    }

    private void PlayCharacterAni()
    {
        
        float moveValue = Input.GetAxis("Horizontal");
        
        if (moveValue != 0)
        {
            lastMoveDirection = moveValue;
            if (moveValue < 0)
            {
                player.Play("left run");
            }
            else if (moveValue > 0)
            {
                player.Play("right run");
            }
            else
            {
                player.Play("right idle");
            }
        }
        player.SetFloat("MoveX", moveValue);

        if (moveValue == 0)
        {
            if (lastMoveDirection < 0)
                player.Play("left idle");
            else
                player.Play("right idle");
        }

    }
}
