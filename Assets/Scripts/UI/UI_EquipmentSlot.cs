//装备插槽UI

using System;
using UnityEngine;

public class UI_EquipmentSlot : UI_ItemSlot
{
    //装备类型
    public EquipmentType slotType;

    private void OnValidate()
    {
        //给对象赋选择值
        gameObject.name = "Equipment Slot - " + slotType.ToString();
    }
}
