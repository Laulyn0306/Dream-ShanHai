using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Sirenix.OdinInspector;

public class SettingCanvas : MonoBehaviour
{
    [BoxGroup("GameData")]
    [Header("GameData")]
    public GameDataSO gameData;

    [BoxGroup("通用")]
    [Header("画布")]
    public GameObject settingCanvas;
    [BoxGroup("通用")]
    public GameObject musicCanvas;
    [BoxGroup("通用")]
    public GameObject screenCanvas;


    [BoxGroup("MusicSetting")]
    [Header("音乐资源与调整")]
    public AudioSource bgmSource;
    [BoxGroup("MusicSetting")]
    public AudioSource sfxSource;

    [BoxGroup("MusicSetting")]
    [Header("滑条")]
    public Slider bgmSlider;
    [BoxGroup("MusicSetting")]
    public Slider sfxSlider;

    [BoxGroup("通用")]
    [Header("按钮")]
    public Button btn_quit;
    [BoxGroup("通用")]
    public Button btn_next;
    [BoxGroup("通用")]
    public Button btn_front;

    [BoxGroup("ScreenSetting")]
    [Header("全屏控制按钮")]
    public Button btn_Left;
    [BoxGroup("ScreenSetting")]
    public Button btn_Right;

    [BoxGroup("ScreenSetting")]
    [Header("全屏中间显示图片")]
    public Image displayImage;

    [BoxGroup("ScreenSetting")]
    [Header("全屏图片资源")]
    public Sprite yes_Sprite; // “是” 图（代表全屏）
    [BoxGroup("ScreenSetting")]
    public Sprite no_Sprite;  // “否” 图（代表窗口）

    [BoxGroup("ScreenSetting")]
    private bool isFullScreen = true;

    [BoxGroup("ScreenSetting")]
    [Header("index")]
    public int currentIndex = 0;

    [BoxGroup("ScreenSetting")]
    [Header("分辨率控制按钮")]
    public Button btn_Leftfbl;
    [BoxGroup("ScreenSetting")]
    public Button btn_Rightfbl;

    [BoxGroup("ScreenSetting")]
    [Header("分辨率中间显示图片")]
    public Image displayImage_fbl;

    [BoxGroup("ScreenSetting")]
    [Header("分辨率图片资源")]
    public Sprite[]  resolutionSprites;

    [BoxGroup("ScreenSetting")]
    [Header("对应的分辨率宽高")]
    public Vector2Int[] resolutions;


    void Awake()
    {

        musicCanvas.SetActive(true);
        screenCanvas.SetActive(false);
        Debug.Log("已设置好");
    }
    // Start is called before the first frame update
    void Start()
    {
        bgmSlider.value = gameData.musicVolume;
        sfxSlider.value = gameData.musicVolume;

        bgmSlider.onValueChanged.AddListener(OnBGMChanged);
        sfxSlider.onValueChanged.AddListener(OnSFXChanged);

        bgmSource.volume = gameData.musicVolume;
        sfxSource.volume = gameData.sfxVolume;
        //音量


        isFullScreen = Screen.fullScreen;
        
        UpdateUIFull();

        UpdateUIFBL();
       
    }

    public void OnBGMChanged(float value)
    {
        gameData.musicVolume = value;
        bgmSource.volume = value;
        gameData.Save();
    }

    public void OnSFXChanged(float value)
    {
        gameData.sfxVolume = value;
        sfxSource.volume = value;
        gameData.Save();
    }
    
    public void CloseSettingCanvas()
    {
        settingCanvas.SetActive(false);
        Debug.Log("111");
    }

    public void SwitchCanvas()
    {
        if (musicCanvas != null) musicCanvas.SetActive(false);
        if (screenCanvas != null) screenCanvas.SetActive(true);
    }

    public void SwitchBackCanvas()
    {
        if (screenCanvas != null) screenCanvas.SetActive(false);

        if (musicCanvas != null) musicCanvas.SetActive(true);
    }

    public void ToggleFullScreen()
    {
        //Debug.Log("按钮点到了！");

        isFullScreen = !isFullScreen;
        if (isFullScreen)
        {
            Screen.fullScreenMode = FullScreenMode.FullScreenWindow;  // 真全屏模式
            Screen.fullScreen = true;
        }
        else
        {
            Screen.fullScreenMode = FullScreenMode.Windowed;         // 窗口模式
            Screen.fullScreen = false;
        }

        
        UpdateUIFull();

        PlayerPrefs.SetInt("isFullScreen", isFullScreen ? 1 : 0);

        Debug.Log(isFullScreen ? "切换为全屏" : "切换为窗口");
    }

    public void UpdateUIFull()
    {
        //Debug.Log("换图片了！");
        if (displayImage != null)
        {
            displayImage.sprite = isFullScreen ? yes_Sprite : no_Sprite;
            
        }
    }

    public void ChangeResolution(int direction)
    {
        currentIndex += direction;

        if (currentIndex < 0) currentIndex = resolutions.Length - 1;
        if (currentIndex >= resolutions.Length) currentIndex = 0;

        UpdateUIFBL();

        SetResolution();
    }

    void UpdateUIFBL()
    {
        if(displayImage_fbl != null && resolutionSprites != null && resolutionSprites.Length > 0)
        {
            displayImage_fbl.sprite = resolutionSprites[currentIndex];
        }
    }

    void SetResolution()
    {
        if (resolutions !=null && currentIndex < resolutions.Length)
        {
            Vector2 res = resolutions[currentIndex];

            Debug.Log($"切换分辨率到：{res.x} x {res.y}");
        }
    }
    public void TestClick()
    {
        Debug.Log("按钮真的被点到了～！");
    }
}
