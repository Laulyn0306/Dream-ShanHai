using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class IntoTalkController : MonoBehaviour
{
    public Button btn_quit;
    // Start is called before the first frame update
    private void Start()
    {
        btn_quit.onClick.AddListener(() => OnclickedBtn_QUIT());
    }

    private void OnclickedBtn_QUIT()
    {
        Destroy(gameObject);
    }
}
