//物品对象

using System;
using UnityEngine;

public class ItemObject : MonoBehaviour
{
   private SpriteRenderer sr;
   //物品数据
   [SerializeField] private ItemData itemData;

   //当脚本的属性在编辑器中被修改时调用
   private void OnValidate()
   {
      //设置图标
      GetComponent<SpriteRenderer>().sprite = itemData.icon;
      //设置名字
      gameObject.name = "ItemObject - "+itemData.itemName;
   }

   private void OnTriggerEnter2D(Collider2D other)
   {
      if (other.GetComponent<Player>() != null)
      {
         //如果玩家碰上--拾取到物品--添加到库存
         Inventory.instance.AddItem(itemData);
         Destroy(gameObject);
      }
   }
}
