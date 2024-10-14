//物品掉落

using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

public class ItemDrop : MonoBehaviour
{
    //可能的物品掉落数
    [SerializeField] private int possibleItemDrop;
    //可能掉落的物品列表
    [SerializeField] private ItemData[] possibleDrop;
    //掉落物品列表
    private List<ItemData> dropList = new List<ItemData>();
    
    //掉落物品预制体
    [SerializeField] private GameObject dropPrefab;

    //生成掉落
    public virtual void GenerateDrop()
    {
        if (possibleDrop.Length==0)
        {
            return;
        }
        for (int i = 0; i < possibleDrop.Length; i++)
        {
            if (Random.Range(0, 100) <= possibleDrop[i].dropChance)
            {
                //添加到掉落物品列表
                dropList.Add(possibleDrop[i]);
            }
        }

        for (int i = 0; i < possibleItemDrop; i++)
        {
            ItemData randomItem = dropList[Random.Range(0, dropList.Count - 1)];
            //只会掉落一件物品
            dropList.Remove(randomItem);
            DropItem(randomItem);
        }
        
    }
    

    //掉落物品
    protected void DropItem(ItemData _itemData)
    {
        //生成预制体对象
        GameObject newDrop = Instantiate(dropPrefab, transform.position, Quaternion.identity);
        
        //随机跳动方向
        Vector2 randomVector = new Vector2(Random.Range(-5, 5), Random.Range(15, 20));
        //掉落物品跳动
        newDrop.GetComponent<ItemObject>().SetupItem(_itemData,randomVector);
    }
}
