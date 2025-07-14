using UnityEngine;

public class GameSceneAutoBinder : MonoBehaviour
{
    void Awake()
    {
        GameScene gameScene = FindObjectOfType<GameScene>();
        if (gameScene == null)
        {
            Debug.LogError("找不到 GameScene 脚本！");
            return;
        }

        // 自动绑定预制体（需要放在 Resources/Prefabs 下）
        gameScene.btn_quitPrefab = Resources.Load<GameObject>("Prefabs/Btn_Quit");
        gameScene.btn_setPrefab = Resources.Load<GameObject>("Prefabs/Btn_Set");
        gameScene.bloodIconPrefab = Resources.Load<GameObject>("Prefabs/BloodIcon");
        gameScene.LevelIntroUIPrefab = Resources.Load<GameObject>("Prefabs/LevelIntroUI");
        gameScene.SSTalkControllerPrefab = Resources.Load<GameObject>("Prefabs/SSTalk");
        gameScene.BlackCanvasPrefab = Resources.Load<GameObject>("Prefabs/BlackCanvas");

        // 自动找场景中物体
        GameObject buttonParentObj = GameObject.Find("ButtonParent");
        if (buttonParentObj != null)
        {
            gameScene.buttonParent = buttonParentObj.transform;
        }
        else
        {
            Debug.LogWarning("找不到 ButtonParent！");
        }

        gameScene.SettingCamvas = GameObject.Find("SettingCanvas");

        Debug.Log("GameScene 自动绑定完成～ 😘");
    }
}
