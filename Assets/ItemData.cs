//物品数据
using UnityEngine;

[CreateAssetMenu(fileName = "新物品数据",menuName = "数据/物品")]
public class ItemData : ScriptableObject
{
    //物品名
    public string itemName;
    //物品图片
    public Sprite icon;
    
}
