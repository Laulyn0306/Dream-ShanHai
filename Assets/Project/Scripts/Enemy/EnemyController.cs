using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;



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

    [Header("血量系统")]
    public float maxHealth = 100f;
    private float currentHealth;
    public Slider healthSlider;

    public GameObject cardPrefab;

    public Transform canvasTransform;

    public GameObject successDialog;

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        player = GameObject.FindGameObjectWithTag("Player").transform;

        enemyID = GetIDFromName(gameObject.name);

        currentHealth = maxHealth;
       

        if (healthSlider != null)
        {
            healthSlider.maxValue = maxHealth;
            healthSlider.value = currentHealth;
        }
    }

    // Update is called once per frame
    void Update()
    {
        //测试受伤逻辑
        if (Input.GetKeyDown(KeyCode.R))
        {
            TakeDamage(25f);
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

        if (!isHurt)
        {
            Vector2 direction = Vector2.zero;

            if (chasingPlayer)
                direction = (player.position - transform.position).normalized;
            else
                direction = (goingToB ? pointB.position - transform.position : pointA.position - transform.position).normalized;

            animator.SetFloat("moveX", direction.x);
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

            player.GetComponent<PlayerController>().isControled = true;
            player.GetComponent<PlayerController>().enemy = this.transform;
            player.GetComponent<PlayerController>().PlayHurtAnimation();
            player.GetComponent<PlayerController>().RestroeControlAfterDelay(0.8f);
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

                PlayerController pc = player.GetComponent<PlayerController>();
                pc.isControled = true;
                pc.enemy = this.transform;
                pc.PlayHurtAnimation();
                pc.RestroeControlAfterDelay(0.8f);


               
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

        animator.SetFloat("moveX", direction.x);

        if (Mathf.Abs(direction.x)>0.01f)
        {
            rb.velocity = new Vector2(direction.x * data.moveSpeed, rb.velocity.y);
            animator.SetBool("IsWalking", true);
            animator.SetFloat("moveX", direction.x);

            if (Mathf.Sign(direction.x) != Mathf.Sign(lastDirectionX))
            {
                if (direction.x < 0)
                {
                    animator.SetTrigger("TurnLeft");
                    Debug.Log("触发转左动画");
                }
                else if (direction.x > 0)
                {
                    animator.SetTrigger("TurnRight");
                    Debug.Log("触发转右动画");
                }

                lastDirectionX = direction.x;
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

    public void TakeDamage(float amount)
    {

        if (isHurt) return;

        currentHealth -= amount;

        if (healthSlider != null)
        {
            healthSlider.value = currentHealth;
        }

        if (currentHealth <= 0)
        {
            Die();
            return;
        }

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
            

        }
        
    }

    private void Die()
    {
        GameObject cardPrefab = GetCardPrefabByID(enemyID);

        if (cardPrefab != null)
        {
            GameObject card = Instantiate(cardPrefab,canvasTransform);
            card.transform.localPosition = Vector3.zero;

            Debug.Log("卡片出现了");
            if (enemyID == "QL" && successDialog != null)
            {
                Debug.Log("当前 enemyID 是：" + enemyID);
               
                StartCoroutine(ShowSuccessDialogAfterDelay(0.5f)); // 你可以自己改这个等待时间
            }
        }
        else
        {
           
        }

        if (healthSlider != null)
        {
            var fade = healthSlider.gameObject.AddComponent<FadeAndDestroy>();
            fade.fadeDuration = 1.5f; // 你可以自定义时长
            fade.StartFade();
        }

        Debug.Log("敌人死了");
        StartCoroutine(DestroyAfter(3f)); // 延迟2秒销毁，等对话框出现

    }

    IEnumerator ShowSuccessDialogAfterDelay(float delay)
    {
        Debug.Log($"⏳ 等待 {delay} 秒开始执行 ShowSuccessDialogAfterDelay");
        yield return new WaitForSeconds(delay);
        Debug.Log("🪄 正在尝试生成成功对话框");

        if (successDialog != null)
        {
            GameObject dialog = Instantiate(successDialog);
           // dialog.transform.SetParent(canvasTransform, false); // 如果你有canvasTransform
            Debug.Log("✅ 成功对话框出现了！");
        }
        else
        {
            Debug.Log("⚠️ successDialog 是空的！！");
        }
    }


    IEnumerator DestroyAfter(float delay)
    {
        
        yield return new WaitForSeconds(delay);
        gameObject.SetActive(false);

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
       



        if (magicAnimator == null)
        {
           
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
       
        return null;
    }

    private GameObject GetCardPrefabByID(string id)
    {
        foreach(var mapping in data.cardMappings)
        {
            if (mapping.enemyID == id)
                return mapping.magicPrefab;
        }
      
        return null;
    }
}
