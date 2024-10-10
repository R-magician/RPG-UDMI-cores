//库存

using System;
using System.Collections.Generic;
using UnityEngine;

public class Inventory : MonoBehaviour
{
    //单例模式
    public static Inventory instance;
    //库存列表
    public List<InventoryItem> inventoryItems;
    //两个参数的公共字典 物品数据--库存物品
    public Dictionary<ItemData, InventoryItem> inventoryDictiatiory;

    [Header("库存UI")] 
    //库存插槽的父节点
    [SerializeField] private Transform inventorySlotParent;

    //插槽list
    private UIItemSlot[] itemSlots; 
    
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

    //更新UI插槽列表
    private void UpdateSlotsUI()
    {
        //获取库存列表
        for (int i = 0; i < inventoryItems.Count; i++)
        {
            //动态的每一个插槽Object更新物品信息
            itemSlots[i].UpdataSlot(inventoryItems[i]);
        }
    }

    private void Start()
    {
        //赋初值
        inventoryItems = new List<InventoryItem>();
        inventoryDictiatiory = new Dictionary<ItemData, InventoryItem>();
        //获取插槽列表
        itemSlots = inventorySlotParent.GetComponentsInChildren<UIItemSlot>();
    }

    //添加库存
    public void AddItem(ItemData _item)
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
            inventoryItems.Add(newItem);
            //添加到库存字典--有数量
            inventoryDictiatiory.Add(_item,newItem);
        }

        //更新插槽UI
        UpdateSlotsUI();
    }

    //移除物品
    public void RemoveItem(ItemData _item)
    {
        //尝试在字典中获取值
        if (inventoryDictiatiory.TryGetValue(_item, out InventoryItem value))
        {
            if (value.stackSize <= 1)
            {
                //清除库存列表--没计数
                inventoryItems.Remove(value);
                //清除字典--有数量
                inventoryDictiatiory.Remove(_item);
            }
            else
            {
                //库存-1
                value.RemoveStack();
            }
        }
        //更新插槽UI
        UpdateSlotsUI();
    }
}
