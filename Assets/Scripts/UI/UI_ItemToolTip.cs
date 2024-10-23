//物品UI 提示

using System;
using TMPro;
using UnityEngine;

public class UI_ItemToolTip : MonoBehaviour
{
    //物品名字
    [SerializeField] private TextMeshProUGUI itemNameText;
    //物品类型
    [SerializeField] private TextMeshProUGUI itemTypeText;
    //物品描述
    [SerializeField] private TextMeshProUGUI itemDescription;

    private void Awake()
    {
        
    }

    //显示提示
    public void ShowToolTip(ItemDataEquipment item)
    {
        itemNameText.text = item.itemName;
        itemTypeText.text = item.equipmentType.ToString(); 
        
        gameObject.SetActive(true);
        
    }

    public void HideToolTip()
    {
        gameObject.SetActive(false);
    }
}
