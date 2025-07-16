using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class SuccessDialogController : MonoBehaviour
{
    [Header("成功图片列表")]
    public List<Sprite> successImages;

    [Header("展示的 Image")]
    public Image successImageDisplay;

    [Header("下一步按钮")]
    public Button nextButton;

    [Header("跳转的场景名")]
    public string nextSceneName = "NextLevel";

    private int currentIndex = 0;

    void Start()
    {
        if (successImages == null || successImages.Count == 0)
        {
            Debug.LogWarning("没有成功图片！");
            return;
        }

        successImageDisplay.sprite = successImages[0];
        nextButton.onClick.AddListener(OnNextClicked);
    }

    void OnNextClicked()
    {
        currentIndex++;

        if (currentIndex >= successImages.Count)
        {
            // 🌀 展示完毕，切场景
            Debug.Log("✨ 成功图片全部播放完毕，跳转场景！");
            SceneManager.LoadScene(nextSceneName);
        }
        else
        {
            successImageDisplay.sprite = successImages[currentIndex];
        }
    }
}
