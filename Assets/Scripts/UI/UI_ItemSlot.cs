//UI-物品插槽

using System;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;

public class UI_ItemSlot : MonoBehaviour , IPointerDownHandler,IPointerEnterHandler,IPointerExitHandler
{
    //显示物品图片组件
    [SerializeField]protected Image itemImage;
    //显示物品名字组件
    [SerializeField]protected TextMeshProUGUI itemText;

    protected UI ui;
    //物品信息
    public InventoryItem item;
    
    //创建一个事件
    public System.Action onRemoveItem;


    protected virtual void Awake()
    {
        ui = GetComponentInParent<UI>();
    }

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
    public virtual void OnPointerDown(PointerEventData eventData)
    {
        if (item == null)
        {
            return;
        }
        
        //移除物品
        if (InputManager.instance.inputControl.Item.Remove.triggered)
        {
            Inventory.instance.RemoveItem(item.data);
            return;
        }
        
        if (item.data.itemType == ItemType.Equipment)
        {
            //装备物品
            Inventory.instance.EquipItem(item.data);
        }
        
        ui.itemToolTip.HideToolTip();
    }

    //鼠标进入执行
    public void OnPointerEnter(PointerEventData eventData)
    {
        if (item == null)
        {
            return;
        }
        
        Vector2 mousePosition = Input.mousePosition;

        float xOffset = 0;
        float yOffset = 0;

        if (mousePosition.x > 600)
        {
            xOffset = -150f;
        }
        else
        {
            xOffset = 150f;
        }

        if (mousePosition.y > 320)
        {
            yOffset = -150f;
        }
        else
        {
            yOffset = 150f;
        }

        
        //显示物品提示
        ui.itemToolTip.ShowToolTip(item.data as ItemDataEquipment);
        ui.itemToolTip.transform.position = new Vector2(mousePosition.x+xOffset, mousePosition.y+yOffset);
    }

    //鼠标退出执行
    public void OnPointerExit(PointerEventData eventData)
    {
        if (item == null)
        {
            return;
        }
        //隐藏提示
        ui.itemToolTip.HideToolTip();
    }
}
