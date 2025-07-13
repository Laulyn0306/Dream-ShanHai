using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;


[System.Serializable]
public class SceneTextImagePair
{
    public string sceneName;
    public List<Sprite> ssNameImage;
}
public class SSTalkController : MonoBehaviour
{

    public GameObject ssTalkPrefab;

    [Header("关卡对话图片映射")]
    public List<SceneTextImagePair> sceneTextImageList;

    private Dictionary<string, List<Sprite>> sceneToSpriteMap;

    [Header("出现的关卡的名字")]
    public List<string> allowedSceneNames;
    // Start is called before the first frame update
    void Start()
    {
        InitMap();
        string currentSceneName = SceneManager.GetActiveScene().name;


        if (!allowedSceneNames.Contains(currentSceneName))
        {
            return;
        }

        if (sceneToSpriteMap.ContainsKey(currentSceneName))
        {

            GameObject dialog = Instantiate(ssTalkPrefab);

            SSTalkHandler handler = dialog.GetComponent<SSTalkHandler>();
            handler.Init(sceneToSpriteMap[currentSceneName]);
        }

    }

    void InitMap()
    {
        sceneToSpriteMap = new Dictionary<string, List<Sprite>>();
        foreach(var pair in sceneTextImageList)
        {
            sceneToSpriteMap[pair.sceneName] = pair.ssNameImage;
        }
    }

    
}
