//库存

using System;
using System.Collections.Generic;
using UnityEngine;

public class Inventory : MonoBehaviour
{
    //单例模式
    public static Inventory instance;

    //装备列表
    public List<InventoryItem> equipment;
    //装备字典
    public Dictionary<ItemDataEquipment, InventoryItem> equipmentDictionary;
    
    //库存列表
    public List<InventoryItem> inventory;
    //两个参数的公共字典 物品数据--库存物品
    public Dictionary<ItemData, InventoryItem> inventoryDictiatiory;
    
    //存储列表
    public List<InventoryItem> stash;
    //物品数据--存储物品
    public Dictionary<ItemData,InventoryItem> stashDictiatiory;

    [Header("库存UI")] 
    //库存插槽的父节点
    [SerializeField] private Transform inventorySlotParent;
    //存放插槽的父节点
    [SerializeField] private Transform stashSlotParent;
    //装备插槽的父节点
    [SerializeField] private Transform equipmentSlotParent;

    //库存插槽list
    private UI_ItemSlot[] inventoryitemSlots; 
    //存放插槽list
    private UI_ItemSlot[] stashitemSlots; 
    //装备插槽list
    private UI_EquipmentSlot[] equipmentitemSlots;
    
    private void Awake()
    {
        if (instance == null)
            instance = this;
        else
        {
            //实例已经存在，要将当前创建的实例给销毁掉
            Destroy(gameObject);
        }
    }

    private void Start()
    {
        //物品列表赋初值
        inventory = new List<InventoryItem>();
        inventoryDictiatiory = new Dictionary<ItemData, InventoryItem>();
        
        //存储列表赋初值
        stash = new List<InventoryItem>();
        stashDictiatiory = new Dictionary<ItemData, InventoryItem>();

        //装备列表赋初值
        equipment = new List<InventoryItem>();
        equipmentDictionary = new Dictionary<ItemDataEquipment, InventoryItem>();
        
        //获取库存插槽列表
        inventoryitemSlots = inventorySlotParent.GetComponentsInChildren<UI_ItemSlot>();
        //获取存储插槽列表
        stashitemSlots = stashSlotParent.GetComponentsInChildren<UI_ItemSlot>();
        //获取装备插槽列表
        equipmentitemSlots = equipmentSlotParent.GetComponentsInChildren<UI_EquipmentSlot>();
    }

    //装备物品
    public void EquipItem(ItemData _item)
    {
        //装备物品--as 类型向下转换
        ItemDataEquipment newEquipment = _item as  ItemDataEquipment;
        InventoryItem newItem = new InventoryItem(newEquipment);

        //用来保存移除的物品
        ItemDataEquipment oldEquipment = null;
        
        //遍历-装备字典的每一项
        foreach (KeyValuePair <ItemDataEquipment,InventoryItem> item in equipmentDictionary)
        {
            if (item.Key.equipmentType == newEquipment.equipmentType)
            {
                //获取是否装备了物品
                oldEquipment = item.Key;
            }
        }

        //如果装备了该物品
        if (oldEquipment != null)
        {
            //卸载装备
            UnequipItem(oldEquipment);
            //重新放回到存储
            AddItem(oldEquipment);
        }
        
        equipment.Add(newItem);
        equipmentDictionary.Add(newEquipment,newItem);
        //移除物品
        RemoveItem(_item);
        
        //更新UI
        UpdateSlotsUI();
    }

    //卸载装备
    private void UnequipItem(ItemDataEquipment itemToDelete)
    {
        if (equipmentDictionary.TryGetValue(itemToDelete, out InventoryItem value))
        {
            //移出
            equipment.Remove(value);
            equipmentDictionary.Remove(itemToDelete);
        }
    }

    //更新UI插槽列表
    private void UpdateSlotsUI()
    {
        for (int i = 0; i < equipmentitemSlots.Length; i++)
        {
            //遍历-装备字典的每一项
            foreach (KeyValuePair <ItemDataEquipment,InventoryItem> item in equipmentDictionary)
            {
                if (item.Key.equipmentType == equipmentitemSlots[i].slotType)
                {
                    //更新插槽数据
                    equipmentitemSlots[i].UpdataSlot(item.Value);
                }
            }
        }
        
        //更新-清理插槽-同步数据到UI
        for (int i = 0; i < inventoryitemSlots.Length; i++)
        {
            inventoryitemSlots[i].ClearUpSlot();
        }
        //更新-清理插槽-同步数据到UI
        for (int i = 0; i < stashitemSlots.Length; i++)
        {
            stashitemSlots[i].ClearUpSlot();
        }
        
        //获取库存列表
        for (int i = 0; i < inventory.Count; i++)
        {
            //动态的每一个插槽Object更新物品信息
            inventoryitemSlots[i].UpdataSlot(inventory[i]);
        }
        
        //获取存储列表
        for (int i = 0; i < stash.Count; i++)
        {
            //动态的每一个插槽Object更新物品信息
            stashitemSlots[i].UpdataSlot(stash[i]);
        }
    }

    //添加库存
    public void AddItem(ItemData _item)
    {
        //装备
        if (_item.itemType == ItemType.Equipment)
        {
            //添加到存储
            AddStash(_item);
        }
        else if(_item.itemType == ItemType.Material)
        {
            //添加到库存
            AddInventory(_item);
        }
        
       

        //更新插槽UI
        UpdateSlotsUI();
    }

    //添加到存储
    private void AddStash(ItemData _item)
    {
        if (stashDictiatiory.TryGetValue(_item, out InventoryItem value))
        {
            value.AddStack();
        }
        else
        {
            //存储物品
            InventoryItem newItem = new InventoryItem(_item);
            //添加到存储列表--没计数
            stash.Add(newItem);
            //添加到存储字典--有数量
            stashDictiatiory.Add(_item,newItem);
        }
    }

    //添加到库存
    private void AddInventory(ItemData _item)
    {
        //尝试在字典中获取值
        if (inventoryDictiatiory.TryGetValue(_item, out InventoryItem value))
        {
            //库存+1;--物品堆叠
            value.AddStack();
        }
        else
        {
            //库存物品
            InventoryItem newItem = new InventoryItem(_item);
            //添加到库存列表--没计数
            inventory.Add(newItem);
            //添加到库存字典--有数量
            inventoryDictiatiory.Add(_item,newItem);
        }
    }

    //移除物品
    public void RemoveItem(ItemData _item)
    {
        //移除库存物品
        RemoveInventory(_item);

        //移除存储物品
        RemoveStash(_item);

        //更新插槽UI
        UpdateSlotsUI();
    }

    //移除存储物品
    private void RemoveStash(ItemData _item)
    {
        //尝试在字典中获取值
        if (stashDictiatiory.TryGetValue(_item, out InventoryItem value))
        {
            if (value.stackSize <= 1)
            {
                //清除库存列表--没计数
                stash.Remove(value);
                //清除字典--有数量
                stashDictiatiory.Remove(_item);
            }
            else
            {
                //库存-1
                value.RemoveStack();
            }
        }
    }
    
    //移除库存物品
    private void RemoveInventory(ItemData _item)
    {
        //尝试在字典中获取值
        if (inventoryDictiatiory.TryGetValue(_item, out InventoryItem value))
        {
            if (value.stackSize <= 1)
            {
                //清除库存列表--没计数
                inventory.Remove(value);
                //清除字典--有数量
                inventoryDictiatiory.Remove(_item);
            }
            else
            {
                //库存-1
                value.RemoveStack();
            }
        }
    }
}
