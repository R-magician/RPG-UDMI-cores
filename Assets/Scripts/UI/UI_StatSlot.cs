//UI显示统计信息节点

using System;
using TMPro;
using UnityEngine;

public class UI_StatSlot : MonoBehaviour
{
    //数据名
    [SerializeField] private string statName;

    [SerializeField] private StatType statType;
    //文本显示值
    [SerializeField] private TextMeshProUGUI statValueText;
    //
    [SerializeField] private TextMeshProUGUI statNameText;

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
    }

    //更新UI上的值
    public void UpdateStatValueUI()
    {
        PlayerStats playerStats = PlayerManager.instance.Player.GetComponent<PlayerStats>();
        if (playerStats != null)
        {
            //给文本赋值
            statValueText.text = playerStats.GetStat(statType).GetValue().ToString();
        }
    }
}
