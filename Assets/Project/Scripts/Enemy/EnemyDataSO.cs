using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class MagicMapping
{
    public string enemyID;
    public GameObject magicPrefab;
}

[CreateAssetMenu(fileName = "EnemyData", menuName = "Enemy/EnemyData", order = 1)]
public class EnemyDataSO : ScriptableObject
{
    public float moveSpeed = 2f;
    public float detectRange = 5f;
    public float attackRange = 1.5f;

    public float hurtDuration = 0.5f;
    public float knockbackForce = 5f;

   
    public float magicSpeed = 5f;
    public float magicCooldown = 2f;

    [Header("敌人id和特效映射")]
    public List<MagicMapping> magicMappings;

    [Header("敌人id和卡片映射")]
    public List<MagicMapping> cardMappings;
}
