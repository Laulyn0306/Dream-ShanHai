using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class EnemyhpSlider : MonoBehaviour
{

    public EnemyHealth enemyHealth;
    public Slider slider;
    // Start is called before the first frame update
    void Start()
    {
        slider.maxValue = enemyHealth.maxHealth;
        slider.value = enemyHealth.currentHealth;
    }

    // Update is called once per frame
    void Update()
    {
        slider.value = enemyHealth.currentHealth;
    }
}
