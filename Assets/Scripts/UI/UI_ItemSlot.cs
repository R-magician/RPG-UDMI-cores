//UI-物品插槽

using System;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.EventSystems;

public class UI_ItemSlot : MonoBehaviour , IPointerDownHandler
{
    //显示物品图片组件
    [SerializeField]private Image itemImage;
    //显示物品名字组件
    [SerializeField]private TextMeshProUGUI itemText;
    //物品信息
    public InventoryItem item;

    //更新插槽数据
    public void UpdataSlot(InventoryItem _item)
    {
        item = _item;
        
        //默认不可见,启用后可见
        itemImage.color = Color.white;
        
        if (item != null)
        {
            //赋值图片
            itemImage.sprite = item.data.icon;
            if (item.stackSize > 1)
            {
                //显示数量
                itemText.text = item.stackSize.ToString();
            }
            else
            {
                //只有一个的时候不显示数量
                itemText.text = "";
            }
        }
    }

    //清理插槽
    public void ClearUpSlot()
    {
        item = null;
        itemImage.sprite = null;
        itemImage.color = Color.clear;
        itemText.text = "";
    }

    //当鼠标点击的时候执行
    public void OnPointerDown(PointerEventData eventData)
    {
        if (item.data.itemType == ItemType.Equipment)
        {
            //装备物品
            Inventory.instance.EquipItem(item.data);
        }
    }
}
