//物品UI 提示

using System;
using TMPro;
using UnityEngine;

public class UI_ItemToolTip : UI_ToolTip
{
    //物品名字
    [SerializeField] private TextMeshProUGUI itemNameText;
    //物品类型
    [SerializeField] private TextMeshProUGUI itemTypeText;
    //物品描述
    [SerializeField] private TextMeshProUGUI itemDescription;

    //默认物品名字体大小
    [SerializeField] private int defaultFontSize = 32;

    //显示提示
    public void ShowToolTip(ItemDataEquipment item)
    {
        if (item == null)
        {
            return;
        }
        
        itemNameText.text = item.itemName;
        itemTypeText.text = item.equipmentType.ToString();
        itemDescription.text = item.GetDescription();

        AdjustPosition();
        AdjustFontSize(itemNameText);
        
        gameObject.SetActive(true);
    }

    public void HideToolTip()
    {
        itemNameText.fontSize = defaultFontSize;
        gameObject.SetActive(false);
    }
}
