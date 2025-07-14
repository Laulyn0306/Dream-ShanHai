using UnityEngine;
using System.Collections;

public class FadeAndDestroy : MonoBehaviour
{
    public float fadeDuration = 1f;

    private CanvasGroup canvasGroup;

    private bool isFading = false;

    void Awake()
    {
        canvasGroup = GetComponent<CanvasGroup>();
        if (canvasGroup == null)
            canvasGroup = gameObject.AddComponent<CanvasGroup>();
        canvasGroup.alpha = 1f; // 初始为可见
    }

    // ✨ 公开方法：外部手动调用
    public void StartFade()
    {
        if (!isFading)
            StartCoroutine(FadeOut());
    }

    private IEnumerator FadeOut()
    {
        isFading = true;

        float timer = 0f;
        Vector3 startPos = transform.position;

        while (timer < fadeDuration)
        {
            timer += Time.deltaTime;
            float t = timer / fadeDuration;
            canvasGroup.alpha = Mathf.Lerp(1f, 0f, t);
            transform.position = startPos + Vector3.up * t * 20f;
            yield return null;
        }

        Destroy(gameObject); // 最后销毁
    }
}
