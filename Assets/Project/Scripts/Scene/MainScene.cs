using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
#if UNITY_EDITOR
using UnityEditor;
#endif

public enum ButtonType
{
    LoadScene,
    CreateSaveCanvas,
    QuitGame,
    ShowSetCanvas,
    
    //
}

[System.Serializable]
public class ButtonScenePair
{
    public Button button;
    public string sceneName;
    public ButtonType buttonType;
}


public class MainScene : MonoBehaviour
{
    public GameDataSO gameData;


    public Transform canvas;

    public RectTransform selector;
   

    [Header("按键与场景映射")]
    public GameObject settingCanvas;
    public GameObject SaveCanvas;
    public List<ButtonScenePair> buttonSceneMappings = new List<ButtonScenePair>();


    [Header("生成的预制体")]
    public GameObject saveCanvas;


    private bool isTransitioning = false;

    
    public AudioSource bgmSource;
    public AudioSource sfxSource;
    // Start is called before the first frame update

    private void Awake()
    {
       
        settingCanvas.SetActive(false);
    }
    void Start()
    {
        

        if (buttonSceneMappings.Count > 0)
        {
            
            MathYPosition(selector, buttonSceneMappings[0].button.transform.position.y);
        }
        
        foreach(var pair in buttonSceneMappings)
        {
            string sceneName = pair.sceneName;
            float targetY = pair.button.transform.position.y;
            ButtonType type = pair.buttonType;

            pair.button.onClick.AddListener(() => OnButtonClick(targetY, sceneName, type));
        }


        gameData.Load();  // 先加载存档数据
        ApplyVolumeSettings();



    }
    void MathYPosition(RectTransform obj, float targetY)
    {
        obj.position = new Vector3(obj.position.x, targetY, obj.position.z);
    }
    void OnButtonClick(float targetY, string sceneName,ButtonType type)
    {

        if (isTransitioning) return;

        isTransitioning = true;
        

        Vector3 targetPos = new Vector3(selector.position.x, targetY, selector.position.z);


        selector.DOKill();

        //把滑片移动按钮的位置
        selector.DOMove(targetPos, 0.3f)
       .SetEase(Ease.OutCubic)
       .OnComplete(() => {
           Debug.Log($"滑片滑完啦～～处理类型：{type}");
           
       });

        switch (type)
        {
            case ButtonType.LoadScene:
                SceneManager.LoadScene(sceneName);
                break;

            case ButtonType.CreateSaveCanvas:
                CreateCanvas();
                break;


            case ButtonType.QuitGame:
                QuitGame();
                break;

            case ButtonType.ShowSetCanvas:
                ShowSetCanvas();
                break;




            default:
                Debug.LogWarning("未知按钮类型！");
                break;
        }

        isTransitioning = false;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void ApplyVolumeSettings()
    {
        bgmSource.volume = gameData.musicVolume;
        sfxSource.volume = gameData.sfxVolume;
    }
    void CreateCanvas()
    {
        if (SaveCanvas == null)
        {
            Debug.LogWarning("亲亲你还没拖预制体进来哦！");
            return;
        }
        SaveCanvas.SetActive(true);
        //gameobject go = instantiate(settingcanvas);

        //go.transform.setparent(canvas, false);

        //go.transform.localposition = vector3.zero;
        //go.transform.localrotation = quaternion.identity;

        //debug.log("创建成功～已经挂在canvas下面啦！");
    }

    void ShowSetCanvas()
    {
        if (settingCanvas != null)
        {
            settingCanvas.SetActive(true);
        }
    }
    void QuitGame()
    {
        Debug.Log("退出游戏");

#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;  // 停止运行
#else
    Application.Quit(); // 正式退出应用
#endif
        
    }


}
