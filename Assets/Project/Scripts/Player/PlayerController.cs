using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
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

    public bool isAttacking = false;
    //人物攻击动画设置

    private bool isWounded = false;
    //人物受伤

    public bool isControled = false;
    public Transform enemy;

    public float climbSpeed = 3f;
    private bool isClimbing = false;
    private float defaultGravityScale;

    public PlayerHealthUI healthUI;

    public bool isDead = false;

    public int maxHealth = 5;
    public float currentHealth;

    private void OnEnable()
    {
        SceneManager.sceneLoaded += OnSceneLoaded;
    }
    private void OnDisable()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }
    // Start is called before the first frame update
    void Start()
    {
        healthUI = PlayerHealthUI.Instance;
        if (healthUI == null)
        {
            Debug.LogError("PlayerHealthUI实例丢了！");
        }
       
        rb = GetComponent<Rigidbody2D>();
        //enemy = GameObject.FindGameObjectWithTag("Enemy").transform;
        defaultGravityScale = rb.gravityScale;
    }

    // Update is called once per frame
    void Update()
    {
        if (isControled)
        {
            rb.velocity = Vector2.zero;
            return;
        }

        if (isWounded)
        {
            return;
        }

        float moveInput = Input.GetAxisRaw("Horizontal");
        
        rb.velocity = new Vector2(moveInput * movespeed, rb.velocity.y);

        isGrounded = Physics2D.OverlapCircle(groundCheck.position, checkRadius, groundLayer)||isClimbing;

        if (Input.GetKeyDown(KeyCode.Space) && isGrounded)
        {
            rb.velocity = new Vector2(rb.velocity.x, jumpForce);
        }

        if (!isAttacking)
        {
            PlayCharacterNoAttack();
            TryAttack();
      
        }

        if (isClimbing)
        {
            float vertical = Input.GetAxisRaw("Vertical");
            rb.velocity = new Vector2(rb.velocity.x, vertical * climbSpeed);
        }

        if (healthUI.currentHealth <= 0 && !isDead)
        {
            isDead = true;
            FindObjectOfType<ScreenFader>().HandleDeath();

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
            StartCoroutine(ResetAttackState());
        }
        if (Input.GetKeyDown(KeyCode.E))
        {
            string direction = lastMoveDirection < 0 ? "left" : "right";
            PlayAttack(direction+" attack2", 1f);
            StartCoroutine(ResetAttackState());
        }
    }

    IEnumerator ResetAttackState()
    {
        yield return new WaitForSeconds(0.3f); // 攻击持续时间
        isAttacking = false;
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

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        if (scene.name == "baihu1")
        {
            currentHealth = maxHealth;
            

            if (healthUI != null)
            {
                healthUI.currentHealth = currentHealth;
                healthUI.UpdateHearts();
            }
            else
            {
                
            }
        }
    }

    public void TakeHit(float delay=0.6f)
    {
        if (!isWounded)
        {
            StartCoroutine(HurtRoutine(delay));

        }
    }

    IEnumerator HurtRoutine(float delay)
    {
        isWounded = true;
        this.enabled = false;
        rb.velocity = Vector2.zero;

        yield return new WaitForSeconds(delay);
        Debug.Log("玩家受伤了");
        player.SetTrigger("Wound");

        yield return new WaitForSeconds(0.4f);
        this.enabled = true;
        isWounded = false;
    }

    public void PlayHurtAnimation()
    {
        if (transform.position.x < enemy.position.x)
        {
            player.SetTrigger("Wound");
        }
    }

    public void RestroeControlAfterDelay(float delay)
    {
        StartCoroutine(Recover(delay));
    }

    private IEnumerator Recover(float delay)
    {
        yield return new WaitForSeconds(delay);
        isControled = false;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (isWounded) return;
        if (collision.CompareTag("Ladder"))
        {
            isClimbing = true;
            rb.gravityScale = 0f;
            rb.velocity = Vector2.zero;
        }

        if (collision.CompareTag("Trap"))
        {
            StartCoroutine(PlayHurtAnimation(1f, 0.5f));
        }

        if (collision.CompareTag("Enemy"))
        {
            healthUI.TakeDamage(1f);
        }
    }

    IEnumerator PlayHurtAnimation(float animDuration,float damage)
    {
        isWounded = true;
        Debug.Log("受伤，-0.5");
        healthUI.TakeDamage(damage);
        player.SetTrigger("Wound");

        yield return new WaitForSeconds(animDuration);
       
        isWounded = false;
        
        
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Ladder"))
        {
            isClimbing = false;
            rb.gravityScale = defaultGravityScale;
        }
    }
    private void OnCollisionEnter2D(Collision2D collision)
    {
        Debug.Log("碰到了：" + collision.gameObject.name);
    }

    
}
