using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;


[CreateAssetMenu(fileName = "PlayerData", menuName = "Laulyn/PlayerData/PlayerData")]

public class PlayerDataSO : ScriptableObject
{
    [Header("玩家状态")]
    public string playerName = "";
    public int playerMaxHP = 5;
    public int playerCurrentHp = 5;
    public int deathCount = 0;
    public class SkillData
    {
        public string skillName;
        public string description;
        public int level;
    }
    public List<SkillData> unlockedSills = new List<SkillData>();
    //玩家名字和hp、技能

    [Header("资源")]
    public int keys = 0;
    //资源

    [Header("关卡信息")]
    public int currentLevel = 1;
    public List<int> unlockedLevels = new List<int>() { 1 };//解锁第一关
    public Dictionary<int, int> levelStars = new Dictionary<int, int>();//(关卡，星极)
    public Vector2 checkPoint = new Vector2(0f, 0f);//关卡重生点
    //关卡状态

    [Header("游戏状态")]
    public bool isPaused = false;
    //游戏状态

    [Header("收集系统")]
    public int collectCount = 0;
    public Dictionary<int, bool> collect = new Dictionary<int, bool>();
    //收集品

    [Header("背包")]
    public Dictionary<string, int> inventory = new Dictionary<string, int>();
    //道具背包（名字，数量）

    public enum SaveSlot
    {
        Slot1,
        Slot2
    }
    public void SaveToSlot(SaveSlot slot)
    {
        string prefix = slot.ToString();

        PlayerPrefs.SetInt($"{prefix}_playerHp", playerCurrentHp);
        PlayerPrefs.SetInt($"{prefix}_deathCount", deathCount);
        PlayerPrefs.SetInt($"{prefix}_keys", keys);
        PlayerPrefs.SetInt($"{prefix}_currentLevel", currentLevel);

        PlayerPrefs.SetFloat(prefix + "checkpointX", checkPoint.x);
        PlayerPrefs.SetFloat(prefix + "checkpointY", checkPoint.y);


        //PlayerPrefs.SetInt("playerHp", playerCurrentHp);

        PlayerPrefs.Save();

        Debug.Log($"✅ 已保存到存档槽 {slot}");
    }

    public void LoadFromSlot(int slot)
    {
        string prefix = slot.ToString();

        playerCurrentHp = PlayerPrefs.GetInt($"{prefix}_playerHp", playerMaxHP);
        deathCount = PlayerPrefs.GetInt($"{prefix}_deathCount", 0);
        keys = PlayerPrefs.GetInt($"{prefix}_keys", 0);
        currentLevel = PlayerPrefs.GetInt($"{prefix}_currentLevel", 0);

        checkPoint = new Vector2(
        PlayerPrefs.GetFloat(prefix + "checkpointX", 0f),
        PlayerPrefs.GetFloat(prefix + "checkpointY", 0f)
         );

        Debug.Log($"📦 从存档槽 {slot} 加载完成！");

    }

    public void AddItem(string itemName, int amount)
    {
        if (inventory.ContainsKey(itemName))
            inventory[itemName] += amount;
        else
            inventory[itemName] = amount;

        Debug.Log($"获得道具：{itemName}*{amount}");
    }
    //添加背包

    public void UnlockLevel(int levelId)
    {
        if (!unlockedLevels.Contains(levelId))
        {
            unlockedLevels.Add(levelId);
            Debug.Log($"解锁了第{levelId}关");
        }
    }
    //切换关卡

    public void GetSkill()
    {
        Debug.Log("获得，，，技能");
    }
    //获得技能

    public void getCollect(int levelID)
    {
        collect[levelID] = true;
        Debug.Log($"收集了{levelID}的东西");
    }
    //收集东西

    public void ResetGame()
    {
        playerCurrentHp = playerMaxHP;
        deathCount = 0;
        keys = 0;
        unlockedSills.Clear();
        unlockedLevels = new List<int> { 1 };
        levelStars.Clear();
        inventory.Clear();
        isPaused = false;
        collect.Clear();
        collectCount = 0;
        Debug.Log("游戏数据已重置");
    }
    //新游戏s
}