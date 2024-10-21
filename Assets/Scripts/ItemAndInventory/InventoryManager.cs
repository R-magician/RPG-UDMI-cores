//库存

using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

public class Inventory : MonoBehaviour
{
    //单例模式
    public static Inventory instance;

    //起始装备
    [FormerlySerializedAs("startingEquipment")] public List<ItemData> startingItems;

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
    //数值卡槽父节点
    [SerializeField] private Transform statSlotParent;

    
    //UI数值节点
    private UI_StatSlot[] statSlots;
    //库存插槽list
    private UI_ItemSlot[] inventoryitemSlots; 
    //存放插槽list
    private UI_ItemSlot[] stashitemSlots; 
    //装备插槽list
    private UI_EquipmentSlot[] equipmentitemSlots;

    [Header("物品冷却")]
    //最后一次使用的时间--药瓶
    private float lastTimeUseFlask;
    //最后一次使用的时间--盔甲
    private float lastTimeUseArmor;

    //药瓶冷却时间
    private float flaskCooldown;
    //盔甲冷却时间
    private float armorCooldown;
    
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

        //获取UI数值结点
        statSlots = statSlotParent.GetComponentsInChildren<UI_StatSlot>();

        //添加起始物品
        AddStartingItems();
    }

    private void AddStartingItems()
    {
        for (int i = 0; i < startingItems.Count; i++)
        {
            AddItem(startingItems[i]);
        }
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
            //重新放回到存储栏
            AddItem(oldEquipment);
        }
        
        equipment.Add(newItem);
        equipmentDictionary.Add(newEquipment,newItem);
        
        //更新装备带给玩家的数值
        newEquipment.AddModifiers();
        
        //移除物品
        RemoveItem(_item);
        
        //更新UI
        UpdateSlotsUI();
    }

    //卸载装备
    public void UnequipItem(ItemDataEquipment itemToRemove)
    {
        if (equipmentDictionary.TryGetValue(itemToRemove, out InventoryItem value))
        {
            //移出
            equipment.Remove(value);
            equipmentDictionary.Remove(itemToRemove);
            //更新装备带给玩家的数值
            itemToRemove.RemoveModifiers();
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
        
        //更新UI上的统计数值
        for (int i = 0; i < statSlots.Length; i++)
        {
            statSlots[i].UpdateStatValueUI();
        }
    }

    //添加库存
    public void AddItem(ItemData _item)
    {
        //装备
        if (_item.itemType == ItemType.Equipment && CanAdddItem())
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

    //限制拾取物品数量
    public bool CanAdddItem()
    {
        if (stash.Count >= stashitemSlots.Length)
        {
            return false;
        }

        return true;
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
    
    //物品可以被合成
    public bool CanCraft(ItemDataEquipment _itemToCraft,List<InventoryItem> _requiredMaterials)
    {
        List<InventoryItem> materialsToRemove = new List<InventoryItem>();
        for (int i = 0; i < _requiredMaterials.Count; i++)
        {
            //库存的物品中有
            if (inventoryDictiatiory.TryGetValue(_requiredMaterials[i].data, out InventoryItem inventoryValue))
            {
                //添加到使用材质
                if (inventoryValue.stackSize<_requiredMaterials[i].stackSize)
                {
                    //没有足够的材质
                    return false;
                }
                else
                {
                    materialsToRemove.Add(inventoryValue);
                }
            }
            else
            {
                //没有足够的材质
                return false;
            }                           
        }

        for (int i = 0; i < materialsToRemove.Count; i++)
        {
            RemoveItem(materialsToRemove[i].data);
        }
        //物品制作完成，添加到插槽中
        AddItem(_itemToCraft);
        return true;
    }

    //获取装备列表
    public List<InventoryItem> GetEquipmentList() => equipment;

    //存储列表
    public List<InventoryItem> GetStashList() => stash;
    
    //获取装备项目
    public ItemDataEquipment GetEquipment(EquipmentType _type)
    {
        ItemDataEquipment equipedItem = null;
        
        //遍历-装备字典的每一项
        foreach (KeyValuePair <ItemDataEquipment,InventoryItem> item in equipmentDictionary)
        {
            if (item.Key.equipmentType == _type)
            {
                //获取是否装备了物品
                equipedItem = item.Key;
            }
        }

        return equipedItem;
    }

    //使用烧瓶
    public void UseFlask()
    {
        //获取烧瓶装备
        ItemDataEquipment currentFlask = GetEquipment(EquipmentType.Flask);

        if (currentFlask == null)
        {
            return;
        }
        
        
        //当前时间 > 上次使用的时间+冷却时间
        bool canUseFlask = Time.time > (lastTimeUseFlask + flaskCooldown);
        //能使用药瓶
        if (canUseFlask)
        {
            flaskCooldown = currentFlask.itemCooldown;
            //无特效
            currentFlask.Effect(null);
            //记录冷却时间
            lastTimeUseFlask = Time.time;
        }
        else
        {
            //在冷却时间
        }
    }
    
    //可以使用盔甲效果
    public bool CanUseArmor()
    {
        //获取当前穿戴盔甲
        ItemDataEquipment currentArmor = GetEquipment(EquipmentType.Armor);

        if (Time.time > lastTimeUseArmor + armorCooldown)
        {
            armorCooldown = currentArmor.itemCooldown;
            lastTimeUseArmor = Time.time;
            return true;
        }

        return false;
    }

    private void Update()
    {
        
    }
}
