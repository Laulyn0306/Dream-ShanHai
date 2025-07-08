using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MyCharacterController : MonoBehaviour
{
    public Transform pointA;
    public Transform pointB;
    //巡逻点

    private bool isTurning = false;
    public float turnPauseDuration = 0.3f;
    //转向

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
    public float knockbackForce = 5f;
   //受伤
    
    private float lastDirectionX = 1f;
    //转向

    public GameObject magicPrefab;
    public Transform magicSpawnPoint;
   
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
        #region 攻击>追击>巡逻
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
        if (isTurning) return;

        Vector3 target = goingToB ? pointB.position : pointA.position;
        

        if (Vector2.Distance(transform.position, target) < 0.1f)
        {
            StartCoroutine(TurnAround());
            return;
        }
        
        MoveTowards(target);
    }
    void MoveTowards(Vector3 target)
    {
        
        Vector2 direction = (target - transform.position).normalized;
        

        if (Mathf.Abs(direction.x)>0.01f)
        {
            rb.velocity = new Vector2(direction.x * moveSpeed, rb.velocity.y);
            animator.SetBool("IsWalking", true);
            animator.SetFloat("moveX", direction.x);

            if (direction.x < 0 && lastDirectionX >= 0)
            {
                animator.SetTrigger("TurnLeft");
                Debug.Log("转左");
            }
            else if (direction.x > 0 && lastDirectionX <= 0)
            {
                animator.SetTrigger("TurnRight");
                Debug.Log("转右");
            }

            lastDirectionX = direction.x;

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

            Vector2 knockbackDir= (transform.position - player.position).normalized;
            rb.AddForce(knockbackForce * knockbackDir, ForceMode2D.Impulse);
            Debug.Log("qq");

        }
        
    }

    IEnumerator TurnAround()
    {
        isTurning = true;

        rb.velocity = Vector2.zero;
        animator.SetBool("IsWalking", false);

        yield return new WaitForSeconds(turnPauseDuration);

        goingToB = !goingToB;

        isTurning = false;
    }

    public void FireMagic()
    {
        GameObject magic = Instantiate(magicPrefab, magicSpawnPoint.position, Quaternion.identity);
        Vector2 dir = (player.position - transform.position).normalized;
        magic.GetComponent<Rigidbody2D>().velocity = dir * 5f;

        Debug.Log("已发射");
    }
}
