//物品数据-装备
using UnityEngine;

public enum EquipmentType
{
    Weapon,//武器
    Armor,//盔甲
    Amulet,//项链-护身符
    Flask,//瓶子
}

[CreateAssetMenu(fileName = "新物品数据",menuName = "数据/装备")]
public class ItemDataEquipment : ItemData
{
    //装备类型
    public EquipmentType equipmentType;
}
