using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyController : MonoBehaviour
{
    public EnemyDataSO data;

    public Transform pointA;
    public Transform pointB;

    //巡逻点

    private bool isTurning = false;
    public float turnPauseDuration = 0.3f;
    //转向

    

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
    private float hurtTime=0f;
    
   //受伤
    
    private float lastDirectionX = 1f;
    //转向

    public Transform magicSpawnPoint;

    [Header("魔法生成偏移距离")]
    public float meleeMagicDistance = 1f;
    public float rangeMagicDistance = 3f;


    private bool canShoot = true;
    private Animation magicAnimator;

    public float magicHitDelay = 0.6f;

    private string enemyID;
    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        player = GameObject.FindGameObjectWithTag("Player").transform;

        enemyID = GetIDFromName(gameObject.name);
        Debug.Log($"自动识别到 enemyID: {enemyID}");
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


        if (distanceToPlayer <= data.attackRange)
        {
            //近战攻击

            animator.SetBool("IsAttacking", true);
            rb.velocity = Vector2.zero;
            animator.SetBool("IsWalking", false);

            
        }
        else if (distanceToPlayer <= data.detectRange)
        {
            animator.SetBool("IsAttacking", false);
            rb.velocity = Vector2.zero;
            animator.SetBool("IsWalking", false);

            if (canShoot)
            {
                FireMagic(false);
                StartCoroutine(ShootCooldown());
                Debug.Log("远程攻击");
            }
           
        }
        else
        {
            animator.SetBool("IsAttacking", false);
            chasingPlayer = false;
            Patrol();
            Debug.Log("巡逻");
        }
            #region 追击

           

                
        
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
            rb.velocity = new Vector2(direction.x * data.moveSpeed, rb.velocity.y);
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
            hurtTime = data.hurtDuration;

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
            rb.AddForce(data.knockbackForce * knockbackDir, ForceMode2D.Impulse);
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

    IEnumerator ShootCooldown()
    {
        canShoot = false;
        yield return new WaitForSeconds(data.magicCooldown);
        canShoot = true;
    }
    public void TriggerMagic(int type)
    {
        if (canShoot)
        {
            bool isMelee = (type == 0);
            FireMagic(isMelee);
            StartCoroutine(ShootCooldown());
        }
    }
    private void FireMagic(int type)
    {
        bool isMelee = (type == 0);
        FireMagic(isMelee);
    }
    public void FireMagic(bool isMelee)
    {
        GameObject magicPrefab = GetMagicPrefabByID(enemyID);

        if (magicPrefab == null || magicSpawnPoint == null||player==null) return;

        Vector3 directionToPlayer = (player.position - transform.position).normalized;

        float offsetDistance = isMelee ? meleeMagicDistance : rangeMagicDistance;

        Vector3 spawPos = player.position + directionToPlayer * offsetDistance;

        GameObject magic = Instantiate(magicPrefab,spawPos, Quaternion.identity);

        //
        magic.transform.right = directionToPlayer;

        Animator magicAnimator = magic.GetComponent<Animator>();
        Debug.Log($"Animator组件: {magicAnimator}");



        if (magicAnimator == null)
        {
            Debug.LogWarning("magicPrefab 没有 Animator！");
            return;
        }

        
        if (isMelee)
        {
            magicAnimator.Play("jing",0,0f);
            Debug.Log("jing");

        }
        else
        {
            magicAnimator.Play("yuan",0,0f);
            Debug.Log("yuan");
        }
        Destroy(magic, 3f);

        

         
    }

    private string GetIDFromName(string name)
    {
        if (string.IsNullOrEmpty(name))
            return"Unknown";

        int index = name.IndexOf("_");

        if (index < 0)
        {
            return name;
        }

        return name.Substring(0, index);
    }

    private GameObject GetMagicPrefabByID(string id)
    {
        foreach (var mapping in data.magicMappings)
        {
            if (mapping.enemyID == id)
                return mapping.magicPrefab;
        }
        Debug.LogWarning($"没找到 ID 为 {id} 的魔法特效，请检查 EnemyDataSO 设置！");
        return null;
    }

    
}
