//UI显示统计信息节点

using System;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;

public class UI_StatSlot : MonoBehaviour,IPointerEnterHandler,IPointerExitHandler
{
    private UI ui;
    
    //数据名
    [SerializeField] private string statName;

    [SerializeField] private StatType statType;
    //文本显示值
    [SerializeField] private TextMeshProUGUI statValueText;
    //
    [SerializeField] private TextMeshProUGUI statNameText;

    [TextArea]
    [SerializeField] private string statDescription;

    private void OnValidate()
    {
        gameObject.name = "Stat - " + statName;

        if (statNameText != null)
        {
            statNameText.text = statName;
            
        }
    }

    private void Start()
    {
        UpdateStatValueUI();
        ui = GetComponentInParent<UI>();
    }

    //更新UI上的值
    public void UpdateStatValueUI()
    {
        PlayerStats playerStats = PlayerManager.instance.Player.GetComponent<PlayerStats>();
        if (playerStats != null)
        {
            //给文本赋值
            statValueText.text = playerStats.GetStat(statType).GetValue().ToString();

            if (statType == StatType.maxHealth)
            {
                statValueText.text = playerStats.GetMaxHealthValue().ToString();
            }
            if (statType == StatType.damage)
            {
                statValueText.text = (playerStats.damage.GetValue() + playerStats.strength.GetValue()).ToString();
            }

            if (statType == StatType.critPower)
            {
                statValueText.text = (playerStats.critPower.GetValue() + playerStats.strength.GetValue()).ToString();
            }
            
            if (statType == StatType.critChance)
            {
                statValueText.text = (playerStats.critChance.GetValue() + playerStats.agility.GetValue()).ToString();
            }
            
            if (statType == StatType.evasion)
            {
                statValueText.text = (playerStats.evasion.GetValue() + playerStats.agility.GetValue()).ToString();
            }
            
            if (statType == StatType.magicResistance)
            {
                statValueText.text = (playerStats.magicResistance.GetValue() + (playerStats.intelligence.GetValue() * 3)).ToString();
            }
        }
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        ui.statToolTip.ShowStatToolTip(statDescription);
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        ui.statToolTip.HideStatToolTip();
    }
}
