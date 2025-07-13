using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ArrowTrapTrigger : MonoBehaviour
{
    public GameObject arrowPrefab; // UI箭Prefab
    public RectTransform canvasTransform; // 指定挂在哪个UI对象下（Canvas子物体）
    public RectTransform[] spawnPoints; // 飞箭的生成点（UI中的坐标）
     
    public Transform playerTransform;
    private bool triggered = false;

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (triggered) return;

        if (other.CompareTag("Player"))
        {
            triggered = true;
            FireArrows();
        }
    }

    void FireArrows()
    {
        //把palyer转坐标
        Vector2 targetUIPos;
        RectTransformUtility.ScreenPointToLocalPointInRectangle(
            canvasTransform,
            Camera.main.WorldToScreenPoint(playerTransform.position),
            Camera.main,
            out targetUIPos
            );
        //发射
        foreach (RectTransform point in spawnPoints)
        {
            //实例化箭
            GameObject arrowGO = Instantiate(arrowPrefab);
            arrowGO.transform.SetParent(canvasTransform, false);

            //设置位置
            RectTransform arrowReact = arrowGO.GetComponent<RectTransform>();
            arrowReact.anchoredPosition = point.anchoredPosition;

            //设置方向
            Vector2 dir = targetUIPos - point.anchoredPosition;
            arrowGO.GetComponent<feijianController>().Init(dir);
        }
    }
}