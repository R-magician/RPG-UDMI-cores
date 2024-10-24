//工艺UI插槽
using UnityEngine.EventSystems;

public class UI_CraftSlot : UI_ItemSlot
{
    protected override void Awake()
    {
        base.Awake();
    }

    //设置材料卡槽
    public void SetupCraftSlot(ItemDataEquipment _data)
    {
        if (_data == null)
        {
            return;
        }
        item.data = _data;
        itemImage.sprite = _data.icon;
        itemText.text = _data.itemName;
    }
    
    // private void OnEnable()
    // {
    //     //更新插槽数据
    //     UpdataSlot(item);
    // }

    public override void OnPointerDown(PointerEventData eventData)
    {
        //装备物品
        ItemDataEquipment craftData = item.data as ItemDataEquipment;

        //检查需要的材料有哪些
        Inventory.instance.CanCraft(craftData, craftData.craftingMaterials);
    }
}