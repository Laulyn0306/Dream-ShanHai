using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class settingCanvas : MonoBehaviour
{
    [Header("按钮")]
    public Button btn_continue;
    public Button btn_save;
    public Button btn_quit;

    [Header("生成的预制体")]
    public GameObject saveCanvas;
    
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    public void btn_quitClicked()
    {
        SceneManager.LoadScene("MainScene");
    }

    public void btn_continueClicked()
    {
        Destroy(gameObject);
    }

    public void btn_saveClicked()
    {
        Destroy(gameObject);
        if (saveCanvas == null)
        {
            Debug.LogWarning("预制体无");
            return;
        }
        GameObject go = Instantiate(saveCanvas);

        go.transform.position = new Vector3(0, 0, 0);
        go.transform.rotation = Quaternion.identity;

        Debug.Log("saveCanvas生成");
    }
}
