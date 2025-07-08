using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

[System.Serializable]
public class ButtonScenePair
{
    public Button button;
    public string sceneName;
}


public class MainScene : MonoBehaviour
{
    public RectTransform selector;

    [Header("按键与场景映射")]
    public List<ButtonScenePair> buttonSceneMappings = new List<ButtonScenePair>();

    
    private bool isTransitioning = false;
    // Start is called before the first frame update
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

            pair.button.onClick.AddListener(() => OnButtonClick(targetY, sceneName));
        }

        
       



    }
    void MathYPosition(RectTransform obj, float targetY)
    {
        obj.position = new Vector3(obj.position.x, targetY, obj.position.z);
    }
    void OnButtonClick(float targetY, string sceneName)
    {

        if (isTransitioning) return;

        isTransitioning = true;
        

        Vector3 targetPos = new Vector3(selector.position.x, targetY, selector.position.z);


        selector.DOKill();

        //把滑片移动按钮的位置
        selector.DOMove(targetPos, 0.3f)
       .SetEase(Ease.OutCubic)
       .OnComplete(() => {
           Debug.Log($"滑片滑完啦～跳转到：{sceneName}");
           SceneManager.LoadScene(sceneName);
       });

    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
