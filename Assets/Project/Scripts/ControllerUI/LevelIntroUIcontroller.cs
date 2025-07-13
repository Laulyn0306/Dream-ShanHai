using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

[System.Serializable]
public class SceneNameImagePair
{
    public string sceneName;
    public Sprite levelNameImage;

}
public class LevelIntroUIcontroller : MonoBehaviour
{

    public GameObject talkPrefab;
   
    [Header("关卡名字图片映射")]
    public List<SceneNameImagePair> sceneNameImageList;

    private Dictionary<string, Sprite> sceneToSpriteMap;


    [Header("出现的关卡名字")]
    public List<string> allowedSceneNames;
    // Start is called before the first frame update
    void Start()
    {
        InitMap();

        string currentScene = SceneManager.GetActiveScene().name;

        if (!allowedSceneNames.Contains(currentScene))
        {
            return;
        }

        if (sceneToSpriteMap.ContainsKey(currentScene))
        {
            ShowDialog(currentScene);

        }
    }
    void InitMap()
    {
        sceneToSpriteMap = new Dictionary<string, Sprite>();
        foreach (var pair in sceneNameImageList)
        {
            sceneToSpriteMap[pair.sceneName] = pair.levelNameImage;

        }
    }
    void ShowDialog(string sceneName)
    {
        GameObject dialog = Instantiate(talkPrefab);

        Image levelNameImage = dialog.transform.Find("text").GetComponent<Image>();
        levelNameImage.sprite = sceneToSpriteMap[sceneName];
        levelNameImage.SetNativeSize();
        levelNameImage.rectTransform.sizeDelta *= 0.8f;

        
           
    }
    // Update is called once per frame
    
}
