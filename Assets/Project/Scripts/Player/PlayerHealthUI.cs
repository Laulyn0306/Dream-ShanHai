using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class PlayerHealthUI : MonoBehaviour
{
    public static PlayerHealthUI Instance;
    public GameObject heartPrefab;
   
    public Sprite fullHeartSprite;
    public Sprite halfHeartSprite;
    public Sprite emptyHeartSprite;


    public int maxHealth = 5;
    public float currentHealth = 5f;

    private List<Image> heartImages = new List<Image>();

    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }
    // Start is called before the first frame update
    void Start()
    {

        InitHearts();
        UpdateHearts();
    }

    // Update is called once per frame
    void InitHearts()
    {
       
        foreach (Transform child in transform)
        {
            Destroy(child.gameObject);
        }

        heartImages.Clear();

        for (int i = 0; i < maxHealth; i++)
        {
            GameObject heart = Instantiate(heartPrefab, transform);
            Image img = heart.GetComponent<Image>();
            if (img == null)
            {
                Debug.LogError("心心Prefab上没挂Image组件！大事不妙！");
            }
            else
            {
                Debug.Log($"成功拿到第{i}个心心的Image组件");
                heartImages.Add(img);
            }
        }
    }

    public void TakeDamage(float amount)
    {
        currentHealth -= amount;
        currentHealth = Mathf.Clamp(currentHealth, 0, maxHealth);
        UpdateHearts();
    }

    public void UpdateHearts()
    {
        Debug.Log($"更新UI，当前心心数量: {heartImages.Count}当前血量: {currentHealth}");
        for (int i = 0; i < heartImages.Count; i++)
        {
            int reverseIndex = heartImages.Count - 1 - i;

            if (i < Mathf.Floor(currentHealth))
            {

                heartImages[reverseIndex].sprite = fullHeartSprite;
            }
            else if (i < currentHealth)
            {
                heartImages[reverseIndex].sprite = halfHeartSprite;
            }
            else
            {
                heartImages[reverseIndex].sprite = emptyHeartSprite;
            }
            Debug.Log($"心 {i} 设置为：{heartImages[reverseIndex].sprite.name}");
        }
       
        Debug.Log("UI 正在更新心心！");
    }
}