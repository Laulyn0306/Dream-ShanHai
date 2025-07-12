using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName ="GameData",menuName ="Laulyn/GameData")]
public  class GameDataSO: ScriptableObject
{
   

    [Header("设置音量")]
    [Range(0f,1f)]
    public  float musicVolume = 0.8f;
    [Range(0f, 1f)]
    public  float sfxVolume = 0.8f;

    [Header("全屏设置")]
    public bool isFullScreen = true;
    //设置
    #region 存档系统
    public  void Save()
    {
        
        PlayerPrefs.SetFloat("musicVolume", musicVolume);
        PlayerPrefs.SetFloat("sfxVolume", sfxVolume);
        PlayerPrefs.SetInt("isFullScreen", isFullScreen ? 1 : 0);


        PlayerPrefs.Save();

        Debug.Log($"✅ 已保存");
    }

    public  void Load()
    {
       
        musicVolume = PlayerPrefs.GetFloat("musicVolume", 0.8f);
        sfxVolume = PlayerPrefs.GetFloat("sfxVolume", 0.8f);
        //playerCurrentHp = PlayerPrefs.GetInt("playerHp", playerCurrentHp);

       

        Debug.Log($"📦 ");

    }
    # endregion
  
}
