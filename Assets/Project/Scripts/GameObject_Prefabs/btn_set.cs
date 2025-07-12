using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class btn_set : MonoBehaviour
{
    public Button btn_set1;

    public GameObject SettingCanvas;

    public void OnClicked()
    {
        if (SettingCanvas == null)
        {
            Debug.LogWarning("亲亲你还没拖预制体进来哦！");
            return;
        }

        GameObject go = Instantiate(SettingCanvas);

        go.transform.position = new Vector3(0, 0, 0);
        go.transform.rotation = Quaternion.identity;

        Debug.Log("🎉 预制体在场景中生成啦，不在 Canvas 下哟～");
    }
}
