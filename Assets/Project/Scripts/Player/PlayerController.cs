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
    //人物的行走和跳跃

    public Animator player;
    private float lastMoveDirection = 1f;
    //人物静止的idle的方向

    private bool isAttacking = false;
    //人物攻击动画设置

    private bool isWounded = false;
    //人物受伤

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void Update()
    {
        if (isWounded)
        {
            return;
        }

        float moveInput = Input.GetAxisRaw("Horizontal");
        
        rb.velocity = new Vector2(moveInput * movespeed, rb.velocity.y);

        isGrounded = Physics2D.OverlapCircle(groundCheck.position, checkRadius, groundLayer);

        if (Input.GetKeyDown(KeyCode.Space) && isGrounded)
        {
            rb.velocity = new Vector2(rb.velocity.x, jumpForce);
        }

        if (!isAttacking)
        {
            PlayCharacterNoAttack();
            TryAttack();
      
        }

          

    }
    #region 播放动画的逻辑
    private void PlayCharacterNoAttack()
    {
        
        float moveValue = Input.GetAxis("Horizontal");

        if (moveValue != 0)
        {
            lastMoveDirection = moveValue;
        }


        player.SetFloat("MoveX", moveValue);


         if (!isGrounded)
         {
            player.Play("jump");
            return;
         }

        if (moveValue != 0)
        {
            if (lastMoveDirection < 0)
                player.Play("left run");
            else
                player.Play("right run");
        }
        else
        {
            if(lastMoveDirection<0) 
                player.Play("left idle");
            else
                player.Play("right idle");

        }

        
    }
    //除了攻击的动作的设置播放
    #region 攻击
    void TryAttack()
    {
        if (Input.GetKeyDown(KeyCode.Q))
        {
            string direction = lastMoveDirection < 0 ? "left" : "right";
            PlayAttack(direction+" attack1", 1f);
        }
        if (Input.GetKeyDown(KeyCode.E))
        {
            string direction = lastMoveDirection < 0 ? "left" : "right";
            PlayAttack(direction+" attack2", 1f);
        }
    }
    void PlayAttack(string animationName,float duration)
    {
        isAttacking = true;
        player.Play(animationName);
        StartCoroutine(AttackCooldown(duration));
    }
    private IEnumerator AttackCooldown(float time)
    {
        isAttacking = true;
        yield return new WaitForSeconds(time);
        isAttacking = false;
    }
    //协程
    #endregion

    #region 受伤
    
    public void TakeDamage()
    {
        if (isWounded) return;

        isWounded = true;
        player.Play("wounded");

        rb.velocity = Vector2.zero;

        StartCoroutine(WoundCooldown(0.5f));
    }

    IEnumerator WoundCooldown(float time)
    {
        yield return new WaitForSeconds(time);
        isWounded = false;
    }
    #endregion
    #endregion
}
