//库存物品--堆叠物品

using System;
using UnityEngine;


//页面可见
[Serializable]
public class InventoryItem
{
    //物品数据
    public ItemData data;
    //堆栈大小--物品数量
    public int stackSize;

    //构造方法
    public InventoryItem(ItemData _itemData)
    {
        data = _itemData;
        //初始化数量+1
        AddStack();
    }

    //添加库存
    public void AddStack()
    {
        stackSize++;
    }
    
    //移除库存
    public void RemoveStack()
    {
        stackSize--;
    }
}
