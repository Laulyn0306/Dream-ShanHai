using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class btn_quit : MonoBehaviour
{
    public Button btn_quit1;

    public void Clicked()
    {
        SceneManager.LoadScene("MainScene");
    }
}
