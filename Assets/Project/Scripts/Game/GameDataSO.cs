using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName ="GameData",menuName ="Laulyn/GameData")]
public  class GameDataSO: ScriptableObject
{
    [Header("玩家状态")]
    public  string playerName = "";
    public  int playerMaxHP=5;
    public  int playerCurrentHp = 5;
    public  int deathCount = 0;
    public class SkillData
    {
        public string skillName;
        public string description;
        public int level;
    }
    public  List<SkillData> unlockedSills = new List<SkillData>();
    //玩家名字和hp、技能

    [Header("资源")]
    public  int keys = 0;
    //资源

    [Header("关卡信息")]
    public  int currentLevel = 1;
    public  List<int> unlockedLevels = new List<int>() { 1 };//解锁第一关
    public  Dictionary<int, int> levelStars = new Dictionary<int, int>();//(关卡，星极)
    public  Vector2 checkPoint = new Vector2(0f, 0f);//关卡重生点
    //关卡状态

    [Header("游戏状态")]
    public  bool isPaused = false;
    //游戏状态

    [Header("收集系统")]
    public  int collectCount = 0;
    public  Dictionary<int, bool> collect = new Dictionary<int, bool>();
    //收集品

    [Header("背包")]
    public  Dictionary<string,int> inventory =new Dictionary<string,int>();
    //道具背包（名字，数量）

    [Header("设置音量")]
    [Range(0f,1f)]
    public  float musicVolume = 0.8f;
    [Range(0f, 1f)]
    public  float sfxVolume = 0.8f;
    //设置
    #region 存档系统
    public  void Save()
    {
        PlayerPrefs.SetInt("playerHp", playerCurrentHp);
        PlayerPrefs.SetInt("deathCount", deathCount);
        PlayerPrefs.SetInt("keys", keys);
        PlayerPrefs.SetInt("currentLevel", currentLevel);
        PlayerPrefs.SetFloat("musicVolume", musicVolume);
        PlayerPrefs.SetFloat("sfxVolume", sfxVolume);
        //PlayerPrefs.SetInt("playerHp", playerCurrentHp);

        PlayerPrefs.Save();
    }

    public  void Load()
    {
        playerCurrentHp = PlayerPrefs.GetInt("playerHp", playerCurrentHp);
        deathCount = PlayerPrefs.GetInt("deathCount", deathCount);
        keys = PlayerPrefs.GetInt("keys", keys);
        currentLevel = PlayerPrefs.GetInt("currentLevel", currentLevel);
        musicVolume = PlayerPrefs.GetFloat("musicVolume", musicVolume);
        sfxVolume = PlayerPrefs.GetFloat("sfxVolume", sfxVolume);
        //playerCurrentHp = PlayerPrefs.GetInt("playerHp", playerCurrentHp);

    }
    # endregion
    public  void AddItem(string itemName,int amount)
    {
        if (inventory.ContainsKey(itemName))
            inventory[itemName] += amount;
        else
            inventory[itemName] = amount;

        Debug.Log($"获得道具：{itemName}*{amount}");
    }
    //添加背包

    public  void UnlockLevel(int levelId)
    {
        if (!unlockedLevels.Contains(levelId))
        {
            unlockedLevels.Add(levelId);
            Debug.Log($"解锁了第{levelId}关");
        }
    }
    //切换关卡

    public  void GetSkill()
    {
        Debug.Log("获得，，，技能");
    }
    //获得技能

    public  void getCollect(int levelID)
    {
        collect[levelID] = true;
        Debug.Log($"收集了{levelID}的东西");
    }
    //收集东西

    public  void ResetGame()
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
    //新游戏
}
