//UI-物品插槽

using System;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class UIItemSlot : MonoBehaviour
{
    //显示物品图片组件
    [SerializeField]private Image itemImage;
    //显示物品名字组件
    [SerializeField]private TextMeshProUGUI itemText;
    //物品信息
    public InventoryItem item;

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
}
