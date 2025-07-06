using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MyCharacterController : MonoBehaviour
{
    public Transform pointA;
    public Transform pointB;
    //巡逻点

    public float moveSpeed = 2f;
    public float detectRange = 5f;
    public float attactRange = 1.5f;
    //

    private Transform player;
    private Rigidbody2D rb;
    private Vector3 targetPoint;
    private bool chasingPlayer = false;
    //

    private Animator animator;
    public LayerMask groundLayer;
    //

    private bool goingToB = true;

    private bool isHurt = false;
    private float hurtDuration = 0.5f;
    private float hurtTime=0f;
   
    
    //受伤
    
    //private float lastMoveDirection = 1f;
    //
   
    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        player = GameObject.FindGameObjectWithTag("Player").transform;
       
    }

    // Update is called once per frame
    void Update()
    {
        //测试受伤逻辑
        if (Input.GetKeyDown(KeyCode.R))
        {
            TakeDamage();
        }

        if (isHurt)
        {
            hurtTime -= Time.deltaTime;
            if (hurtTime <= 0f)
            {
                isHurt = false;

                
            }
            return;
        }
        #region 先攻击再追击最后巡逻
        if (player == null)
        {
            player = GameObject.FindGameObjectWithTag("Player")?.transform;
            if (player == null) return;
        }

        float distanceToPlayer = Vector2.Distance(transform.position, player.position);

        if (distanceToPlayer <= attactRange)
        {

            animator.SetBool("IsAttacking", true);
            rb.velocity = Vector2.zero;
            animator.SetBool("IsWalking", false);
            Debug.Log("开始攻击");
        }
        else
        {
            animator.SetBool("IsAttacking", false);


            #region 追击

            if (distanceToPlayer <= detectRange)
            {
                //追击
                chasingPlayer = true;
                MoveTowards(player.position);
                Debug.Log("开始追击");
            }

            else
            {
                //巡逻
                chasingPlayer = false;
                Debug.Log("开始巡逻");
                Patrol();
            }

                
        }
        #endregion




        if (chasingPlayer)
           MoveTowards(player.position);
        else
            Patrol();
        #endregion

      
    }

    void Patrol()
    {
        Vector3 target = goingToB ? pointB.position : pointA.position;
        MoveTowards(target);

        if (Vector2.Distance(transform.position, target) < 0.2f)
        {
            goingToB = !goingToB;
        }
    }
    void MoveTowards(Vector3 target)
    {
        
        Vector2 direction = (target - transform.position).normalized;
        //spriteRenderer.flipX = direction.x < 0;

        if (Mathf.Abs(direction.x)>0.01f)
        {
            rb.velocity = new Vector2(direction.x * moveSpeed, rb.velocity.y);
            animator.SetBool("IsWalking", true);
            animator.SetFloat("moveX", direction.x);
        }

        else
        {
            rb.velocity = Vector2.zero;
            animator.SetBool("IsWalking", false);
        }

        
    }

    private void OnDisable()
    {
        if (animator != null)
        {
            animator.SetBool("IsWalking", false);
        }
    }

    private void TakeDamage()
    {
        if (!isHurt)//防止重复播放
        {

            
            isHurt = true;
            hurtTime = hurtDuration;

            //停止移动
            rb.velocity = Vector2.zero;
            animator.SetBool("IsWalking", false);
            animator.SetBool("IsAttacking", false);

            
           

            if (animator.GetFloat("moveX") < 0)
            {
                animator.SetTrigger("HurtLeft");
                Debug.Log("左受伤");
            }

            if (animator.GetFloat("moveX") > 0)
            {
                animator.SetTrigger("HurtRight");
                Debug.Log("右受伤");
            }

        }
        
    }
}
