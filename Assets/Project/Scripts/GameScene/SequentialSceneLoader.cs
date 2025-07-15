using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SequentialSceneLoader : MonoBehaviour
{

    [Header("按顺序写场景名")]
    public string[] sceneOrder;

    [Header("当前是第几关")]
    public int currentSceneIndex = 0;


   

    private void Start()
    {
        // 找到当前场景索引
        string currentScene = SceneManager.GetActiveScene().name;
        currentSceneIndex = System.Array.IndexOf(sceneOrder, currentScene);
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            // 判断下一个场景索引
            int nextSceneIndex = currentSceneIndex + 1;
            if (nextSceneIndex < sceneOrder.Length)
            {
                SceneManager.LoadScene(sceneOrder[nextSceneIndex]);
            }
            else
            {
                Debug.Log("已经是最后一关啦！");
                // 你想加结算界面啥的也行~
            }
        }
    }
}


