using UnityEngine;

public class PlayerAttackTrigger : MonoBehaviour
{
    private PlayerController player;

    void Start()
    {
        player = GetComponentInParent<PlayerController>();
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (player.isAttacking && other.CompareTag("Enemy"))
        {
            EnemyController enemy = other.GetComponent<EnemyController>();
            if (enemy != null)
            {
                enemy.TakeDamage(25f); // 💥敌人掉血
                Debug.Log("攻击命中敌人！");
            }
        }
    }
}
