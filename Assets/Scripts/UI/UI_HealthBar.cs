//血量UI

using System;
using UnityEngine;
using UnityEngine.UI;

public class UI_HealthBar : MonoBehaviour
{
    private Enity enity;
    //数值统计
    private CharacterStats myStats;
    //UI 元素的大小、锚点、偏移量和位置
    private RectTransform myTransform;
    private Slider slider;

    private void Start()
    {
        enity = GetComponentInParent<Enity>();
        myStats = GetComponentInParent<CharacterStats>();
        myTransform = GetComponent<RectTransform>();
        slider = GetComponentInChildren<Slider>();

        //事件订阅
        enity.onFlipped += FlipUI;
        myStats.onHealthChanged += UpdateHealthUI;

        UpdateHealthUI();
    }
    
    //翻转UI
    private void FlipUI()
    {
        //血条翻转，保证血条一直是一个方向
        myTransform.Rotate(0,180,0);
    }

    //更新血量UI
    private void UpdateHealthUI()
    {
        //当前血量=最大生命值+活力
        slider.maxValue = myStats.GetMaxHealthValue();
        slider.value = myStats.currentHealth;
    }

    private void OnDisable()
    {
        //注销事件
        enity.onFlipped -= FlipUI;
        myStats.onHealthChanged -= UpdateHealthUI;
    }
}
