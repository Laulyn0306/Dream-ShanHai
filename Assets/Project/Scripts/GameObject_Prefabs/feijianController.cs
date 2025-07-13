using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class feijianController : MonoBehaviour
{

    public float speed=300f;
    public Vector2 direction;
    public RectTransform rect;

    public void Init(Vector2 dir)
    {

        rect = GetComponent<RectTransform>();
        direction = dir.normalized;

        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
        rect.rotation = Quaternion.Euler(0, 0, angle);
    }
  
    void Update()
    {
        if (rect == null) return;

        rect.anchoredPosition += direction * speed * Time.deltaTime;

        
    }

    //血量伤害
}
