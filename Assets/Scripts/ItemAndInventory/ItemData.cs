//物品数据

using System;
using System.Text;
using UnityEditor;
using UnityEngine;

public enum ItemType
{
    Material,//材料
    Equipment,//装备
}

[CreateAssetMenu(fileName = "新物品数据",menuName = "数据/物品")]
public class ItemData : ScriptableObject
{
    public ItemType itemType;
    //物品名
    public string itemName;
    //物品图片
    public Sprite icon;
    //物品id
    public string itemId;

    [Range(0,100)]
    //掉落概率
    public float dropChance;

    protected StringBuilder sb = new StringBuilder();

    //获取详情
    public virtual string GetDescription()
    {
        return "";
    }

    private void OnValidate()
    {
        //AssetDatabase：仅在编辑器中可用，在运行时无法使用。
        //在编辑器中才执行
        #if UNITY_EDITOR
        string path = AssetDatabase.GetAssetPath(this);
        //获取唯一id
        itemId = AssetDatabase.AssetPathToGUID(path);
        #endif
    }
}
