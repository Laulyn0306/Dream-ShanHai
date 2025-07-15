using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class IntoTalkController : MonoBehaviour
{
    
    public Image textImage;
    public Button btn_next;

    private List<Sprite> dialogSprites;
    private int currentIndex = 0;


    public void Init(List<Sprite> sprites)
    {
        dialogSprites = sprites;
        currentIndex = 0;

        ShowCurrent();

        btn_next.onClick.AddListener(ShowNext);
    }

    void ShowCurrent()
    {
        if (currentIndex < dialogSprites.Count)
        {
            textImage.sprite = dialogSprites[currentIndex];
            textImage.SetNativeSize();
        }
    }

    void ShowNext()
    {
        currentIndex++;

        if (currentIndex < dialogSprites.Count)
        {
            ShowCurrent();
        }
        else
        {
            Destroy(gameObject);
        }
    }
}


