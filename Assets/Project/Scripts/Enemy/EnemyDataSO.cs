using UnityEngine;

[CreateAssetMenu(fileName = "EnemyData", menuName = "Enemy/EnemyData", order = 1)]
public class EnemyDataSO : ScriptableObject
{
    public float moveSpeed = 2f;
    public float detectRange = 5f;
    public float attackRange = 1.5f;

    public float hurtDuration = 0.5f;
    public float knockbackForce = 5f;

    public GameObject magicPrefab;
    public float magicSpeed = 5f;
    public float magicCooldown = 2f;
}
