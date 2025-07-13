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

    [Header("生成位置")]
    public Transform buttonParent;
    // Start is called before the first frame update
    private void Awake()
    {
        CreateButton();
        CreateLevelIntroUI();
        CreateSSTalkController();
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

    void CreateLevelIntroUI()
    {
        GameObject levelIntroUI = Instantiate(LevelIntroUIPrefab);
    }

    void CreateSSTalkController()
    {
        GameObject SSTalkController = Instantiate(SSTalkControllerPrefab);
    }
}
