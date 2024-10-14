//玩家物品掉落

using System.Collections.Generic;
using UnityEngine;

public class PlayerItemDrop : ItemDrop
{
    [Header("玩家掉落物品")]
    //丢失物品的概率
    [SerializeField] private float chanceToLooseItems;

    //丢失材料的概率
    [SerializeField] private float chanceToLooseMaterials;

    //生成掉落
    public override void GenerateDrop()
    {
        //获取实例
        Inventory inventory = Inventory.instance;
        
        //移除的物品的列表
        List<InventoryItem> itemsToUnequip = new List<InventoryItem>();
        //材料丢失的列表
        List<InventoryItem> materuialToLoose = new List<InventoryItem>();

        //对每一个物品检查丢失--装备物品列表
        foreach (InventoryItem item in inventory.GetEquipmentList())
        {
            if (Random.Range(0, 100) <= chanceToLooseItems)
            {
                //丢失物品
                DropItem(item.data);
                itemsToUnequip.Add(item);
            }
        }

        for (int i = 0; i < itemsToUnequip.Count; i++)
        {
            //卸载装备
            inventory.UnequipItem(itemsToUnequip[i].data as ItemDataEquipment); 
        }

        
        //获取存储列表
        foreach (InventoryItem item in  inventory.GetStashList())
        {
            if (Random.Range(0, 100) <= chanceToLooseMaterials)
            {
                //掉落物品
                DropItem(item.data);
                materuialToLoose.Add(item);
            }
        }
        
        for (int i = 0; i < materuialToLoose.Count; i++)
        {
            //移除物品
            inventory.RemoveItem(materuialToLoose[i].data);
        }
    }
}
