//工艺UI插槽

using System;
using UnityEngine;
using UnityEngine.EventSystems;

public class UI_CraftSlot : UI_ItemSlot
{
    private void OnEnable()
    {
        //更新插槽数据
        UpdataSlot(item);
    }

    public override void OnPointerDown(PointerEventData eventData)
    {
       ItemDataEquipment craftData = item.data as ItemDataEquipment;

       Inventory.instance.CanCraft(craftData, craftData.craftingMaterials);
    }
}
