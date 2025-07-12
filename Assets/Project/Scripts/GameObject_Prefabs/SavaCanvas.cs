using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

[System.Serializable]
public class SceneSpritePair
{
    public string sceneName;
    public Sprite levelImage;
}

public class SavaCanvas : MonoBehaviour
{
    [Header("关卡图标映射")]
    public List<SceneSpritePair> sceneImageMappings;

    

    [Header("显示的Image组件（存档槽的图）")]
    public Image levelImageDisplay;

    [Header("存档槽按钮列表")]
    public List<Button> saveSlotButtons;

    [Header("退出按钮")]
    public Button btn_quit;

    [Header("图片到场景的映射")]
    public List<SceneSpritePair> imageSceneMappings;

    private Button clickedSlot = null;
    void Start()
    {
        string currentSceneName = UnityEngine.SceneManagement.SceneManager.GetActiveScene().name;
        Debug.Log($"当前场景名是：{currentSceneName}");

        for (int i = 0; i < saveSlotButtons.Count; i++)
        {
            Button btn = saveSlotButtons[i];
         
                int index = i;
                btn.onClick.AddListener(() => OnClickSaveSlot(btn, index));

            Transform location = btn.transform.Find("location");
            Image img = location != null ? location.GetComponent<Image>() : null;

            if (img != null)
            {
                img.gameObject.SetActive(false);
            }

            string savedScene = PlayerPrefs.GetString($"SaveSlot_{i}", "");
            if(!string.IsNullOrEmpty(savedScene))
            {
                foreach(var pair in imageSceneMappings)
                {
                    if (pair.sceneName == savedScene)
                    {
                        if (img != null)
                        {

                            img.sprite = pair.levelImage;
                            img.gameObject.SetActive(true);

                            Debug.Log($"✅ 恢复槽{i}的图片为：{pair.levelImage.name}");
                        }

                        break;

                        
                    }
                }
            }
            else
            {
                Debug.Log($"槽{i}暂无存档记录，显示为空");
            }
        }

        if (btn_quit != null)
        {
            btn_quit.onClick.AddListener(() => OnclickBtn_quit());
        }

        
        
    }

    private void OnClickSaveSlot(Button slotButton,int slotIndex)
    {

        string currentSceneName = SceneManager.GetActiveScene().name;
        Debug.Log($"{currentSceneName}");

        //主菜单打开
        if (currentSceneName == "MainScene")
        {
            Transform location = slotButton.transform.Find("location");
            if (location != null)
            {
                Image img = location.GetComponent<Image>();
                if (img != null && img.sprite != null)
                {
                    foreach(var mapping in imageSceneMappings)
                    {
                        if (mapping.levelImage == img.sprite)
                        {
                            Debug.Log($"跳转到场景：{mapping.sceneName}");
                            SceneManager.LoadScene(mapping.sceneName);
                            return;
                        }
                    }
                    Debug.LogWarning("找不到图片对应的场景！");
                }
                else
                {
                    Debug.Log("这个存档槽没图，不能跳哦～😝");
                }
            }
            return;
        }

        //GameScene
        if (clickedSlot != null)
        {
            Debug.Log("已经点过了");
            return;
        }

        clickedSlot = slotButton;

        bool matched = false;


        foreach (var pair in sceneImageMappings)
        {
            if (pair.sceneName == currentSceneName)
            {

                Transform locationChild = slotButton.transform.Find("location");

                if (locationChild != null)
                {
                    Image img = locationChild.GetComponent<Image>();
                    if (img != null)
                    {
                        img.sprite = pair.levelImage;
                        img.gameObject.SetActive(true);
                        Debug.Log($"成功设置location图标为：{pair.levelImage.name}");

                        //存储信息

                        PlayerPrefs.SetString($"SaveSlot_{slotIndex}", currentSceneName);
                        PlayerPrefs.Save();
                        Debug.Log($"已保存 SaveSlot_{slotIndex} 对应 {currentSceneName}");

                        matched = true;

                    }
                }
                break;

            }
        }
        if (matched) 
        { 
            foreach(Button btn in saveSlotButtons)
            {
                btn.interactable = false;
            }

            Debug.Log($"玩家点了存档槽：{slotButton.name}，其他按钮全部禁用！");
        }
            
            
        }

        
    

    private void OnclickBtn_quit()
    {
        gameObject.SetActive(false);
    }
}

