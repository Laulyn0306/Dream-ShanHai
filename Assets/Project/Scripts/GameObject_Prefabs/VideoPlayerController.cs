using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Video;
using UnityEngine.SceneManagement;
using System;

public class VideoPlayerController : MonoBehaviour
{
    public VideoPlayer videoPlayer;
    

    private void Start()
    {
        videoPlayer.loopPointReached += OnVideoFinished;
        videoPlayer.Play();
    }

    
    void OnVideoFinished(VideoPlayer vp)
    {
        SceneManager.LoadScene("baihu1");
    }
}
