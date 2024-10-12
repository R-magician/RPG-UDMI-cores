//装备插槽UI

using System;
using UnityEngine;
using UnityEngine.EventSystems;

public class UI_EquipmentSlot : UI_ItemSlot
{
    //装备类型
    public EquipmentType slotType;

    private void OnValidate()
    {
        //给对象赋选择值
        gameObject.name = "Equipment Slot - " + slotType.ToString();
    }

    public override void OnPointerDown(PointerEventData eventData)
    {
        //卸载装备物品
        Inventory.instance.UnequipItem(item.data as ItemDataEquipment);
        //添加到存储栏
        Inventory.instance.AddItem(item.data as ItemDataEquipment);
        //清理插槽
        ClearUpSlot();
    }
}
