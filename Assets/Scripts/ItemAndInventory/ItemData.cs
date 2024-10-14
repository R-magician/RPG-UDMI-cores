//物品数据
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

    [Range(0,100)]
    //掉落概率
    public float dropChance;

}
