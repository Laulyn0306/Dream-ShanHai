using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameScene : MonoBehaviour
{
    [Header("预制体按钮")]
    public GameObject btn_quitPrefab;
    public GameObject btn_setPrefab;

    [Header("预制体面板")]
    public GameObject SettingCamvas;

    [Header("预制体IntroUI控制器")]
    public GameObject LevelIntroUIPrefab;

    [Header("预制体SSTalk控制器")]
    public GameObject SSTalkControllerPrefab;

    [Header("预制体血条")]
    public GameObject bloodIconPrefab;

    [Header("生成位置")]
    public Transform buttonParent;

    [Header("预制体黑屏的Canvas")]
    public GameObject BlackCanvasPrefab;

    private ScreenFader screenFader;
    // Start is called before the first frame update
    private void Awake()
    {
        CreateButton();
        CreateLevelIntroUI();
        CreateSSTalkController();
        CreateBloodIcon();
        CreateBlackCanvas();
    }
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void CreateButton()
    {
        GameObject btn_quit = Instantiate(btn_quitPrefab);
        btn_quit.transform.SetParent(buttonParent, false);
        btn_quit.GetComponent<RectTransform>().anchoredPosition = new Vector2(0, 10);

        GameObject btn_set = Instantiate(btn_setPrefab);
        btn_set.transform.SetParent(buttonParent, false);
        btn_set.GetComponent<RectTransform>().anchoredPosition = new Vector2(0, -10
            );

    }
    void CreateBloodIcon()
    {
        GameObject BloodIcon = Instantiate(bloodIconPrefab);
        BloodIcon.transform.SetParent(buttonParent, false);
        BloodIcon.GetComponent<RectTransform>().anchoredPosition = new Vector2(65, 10);

        var bloodscript = BloodIcon.GetComponent<PlayerHealthUI>();
        bloodscript.maxHealth = 5;
        Debug.Log("111");
    }
    void CreateLevelIntroUI()
    {
        GameObject levelIntroUI = Instantiate(LevelIntroUIPrefab);
    }

    void CreateSSTalkController()
    {
        GameObject SSTalkController = Instantiate(SSTalkControllerPrefab);
    }

    void CreateBlackCanvas()
    {
        GameObject BlackCanvas = Instantiate(BlackCanvasPrefab);
        screenFader = BlackCanvas.GetComponent<ScreenFader>();
        if (screenFader == null)
        {
            screenFader = BlackCanvas.AddComponent<ScreenFader>();
        }
    }
}
