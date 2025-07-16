using UnityEngine;
using UnityEngine.Video;
using UnityEngine.SceneManagement;

public class VideoEndToNextScene : MonoBehaviour
{
    public VideoPlayer videoPlayer;   // 拖你的VideoPlayer进来
    public string nextSceneName;      // 要切换的场景名

    void Start()
    {
        // 注册事件，当视频播放完调用
        videoPlayer.loopPointReached += OnVideoEnd;
    }

    void OnVideoEnd(VideoPlayer vp)
    {
        SceneManager.LoadScene(nextSceneName);  // 切场景！
    }
}
