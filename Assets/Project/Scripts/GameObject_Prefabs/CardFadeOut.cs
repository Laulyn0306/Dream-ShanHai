using UnityEngine;
using UnityEngine.UI;

public class CardFadeOut : MonoBehaviour
{
    public float delay = 3f;           // 停留时间
    public float fadeDuration = 1f;    // 淡出时间

    private CanvasGroup canvasGroup;

    void Start()
    {
        canvasGroup = GetComponent<CanvasGroup>();

        if (canvasGroup == null)
        {
            canvasGroup = gameObject.AddComponent<CanvasGroup>();
        }

        StartCoroutine(FadeOutAfterDelay());
    }

    private System.Collections.IEnumerator FadeOutAfterDelay()
    {
        yield return new WaitForSeconds(delay);

        float timer = 0f;
        while (timer < fadeDuration)
        {
            timer += Time.deltaTime;
            canvasGroup.alpha = Mathf.Lerp(1f, 0f, timer / fadeDuration);
            yield return null;
        }

        Destroy(gameObject); // 最后删掉自己
    }
}
