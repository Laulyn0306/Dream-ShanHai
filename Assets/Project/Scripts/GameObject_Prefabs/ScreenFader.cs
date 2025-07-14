using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class ScreenFader : MonoBehaviour
{
    public string restartSceneName = "gameScene1"; // 重启目标场景

    private bool isDead = false;
    private CanvasGroup canvasGroup;

    private void Awake()
    {
        canvasGroup = GetComponentInChildren<CanvasGroup>();
        if (canvasGroup == null)
        {
            Debug.LogError("BlackScreenUI 缺少 CanvasGroup！");
        }
    }

    public void HandleDeath()
    {
        if (!isDead)
        {
            isDead = true;
            StartCoroutine(PlayDeathBlackScreen());
        }
    }

    IEnumerator PlayDeathBlackScreen()
    {
        if (canvasGroup == null)
            yield break;

        float duration = 1.5f;
        float timer = 0f;
        while (timer < duration)
        {
            timer += Time.deltaTime;
            canvasGroup.alpha = Mathf.Lerp(0f, 1f, timer / duration);
            yield return null;
        }
        canvasGroup.alpha = 1f;

        yield return new WaitForSeconds(1f);

        SceneManager.LoadScene(restartSceneName);

        Destroy(gameObject);
        Debug.Log("yixiaohui");
    }

}
