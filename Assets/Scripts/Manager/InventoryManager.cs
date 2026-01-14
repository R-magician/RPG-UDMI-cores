//库存
using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEngine;
using UnityEngine.Serialization;

[DefaultExecutionOrder(1000)]
public class Inventory : MonoBehaviour,ISaveManager
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
    public float flaskCooldown { get; private set; }
    //盔甲冷却时间
    private float armorCooldown;

    [Header("基础数据")]
    //加载物品
    public List<InventoryItem> loadedItems;
    //装备物品
    public List<ItemDataEquipment> LoadedEquipments;
    
    
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

    //添加起始物品
    private void AddStartingItems()
    {
        foreach (ItemDataEquipment item in LoadedEquipments)
        {
            EquipItem(item);
        }
        
        if (loadedItems.Count > 0)
        {
            foreach (InventoryItem item in loadedItems)
            {
                for (int i = 0; i < item.stackSize; i++)
                {
                    AddItem(item.data);
                }
            }
            return;
        }
        
        for (int i = 0; i < startingItems.Count; i++)
        {
            if (startingItems[i] != null)
            {
                AddItem(startingItems[i]);
            }
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
        
        UpdateStatsUI();
    }

    public void UpdateStatsUI()
    {
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

    public void LoadData(GameData _data)
    {
        // GetItemDataBase();
        foreach (KeyValuePair<string,int> pair in _data.inventory)
        {
            foreach (var item in GetItemDataBase())
            {
                if (item != null && item.itemId == pair.Key)
                {
                    InventoryItem itemToLoad = new InventoryItem(item);
                    itemToLoad.stackSize = pair.Value;
                    loadedItems.Add(itemToLoad);
                }
            }
        }

        foreach (string loadedItemId in _data.equipmentId)
        {
            foreach (var item in GetItemDataBase())
            {
                if (item != null && loadedItemId == item.itemId)
                {
                    LoadedEquipments.Add(item as ItemDataEquipment);
                }
            }
        }
    }

    public void SaveData(ref GameData _data)
    {
        //清除字典--库存列表
        _data.inventory.Clear();
        //清除列表-存储列表
        _data.equipmentId.Clear();

        foreach (KeyValuePair<ItemData,InventoryItem> pair in inventoryDictiatiory)
        {
            //保存库存数量
            _data.inventory.Add(pair.Key.itemId,pair.Value.stackSize);
        }

        foreach (KeyValuePair<ItemData,InventoryItem> pair in stashDictiatiory)
        {
            //保存存储数量
            _data.inventory.Add(pair.Key.itemId,pair.Value.stackSize);
        }

        foreach (KeyValuePair<ItemDataEquipment,InventoryItem> pair in equipmentDictionary)
        {
            _data.equipmentId.Add(pair.Key.itemId);
        }
    }

    //获取物品的基础数据
    private List<ItemData> GetItemDataBase()
    {
        //数据被清空并重新创建，以保证没有旧数据残留
        List<ItemData> itemDataBase = new List<ItemData>();
        //查找资源 GUID--返回 Assets/Data/Material 文件夹下所有资源的 GUID
        //AssetDatabase：仅在编辑器中可用，在运行时无法使用。
        //会将文件夹也加入
        string[] assetNames1 = AssetDatabase.FindAssets("", new[] { "Assets/Data/Material" });
        string[] assetNames2 = AssetDatabase.FindAssets("", new[] { "Assets/Data/Equipment" });
        string[] assetNames = assetNames1.Concat(assetNames2).ToArray();
        
        foreach (string SOName in assetNames)
        {
            //将每个资源的 GUID 转换为文件路径
            var SOpath = AssetDatabase.GUIDToAssetPath(SOName);
            //根据路径加载资源，并将它转换为 ItemData 类型的对象
            var itemData = AssetDatabase.LoadAssetAtPath<ItemData>(SOpath);
            
            if (itemData != null)
            {
                itemDataBase.Add(itemData);
            }
        }
        return itemDataBase;
    }
}
