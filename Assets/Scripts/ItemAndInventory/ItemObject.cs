//物品对象

using System;
using UnityEngine;

public class ItemObject : MonoBehaviour
{
   [SerializeField] private Rigidbody2D rb;
   //物品数据
   [SerializeField] private ItemData itemData;
   
   //设置物品
   public void SetupItem(ItemData _itemData,Vector2 _velocity)
   {
      itemData = _itemData;

      rb.linearVelocity = _velocity;
      
      //设置属性
      SetupVisuals();
   }
   
   private void SetupVisuals()
   {
      if (itemData == null)
      {
         return;
      }
      //设置图标
      GetComponent<SpriteRenderer>().sprite = itemData.icon;
      //设置名字
      gameObject.name = "ItemObject - "+itemData.itemName;
   }

   //拾取物品
   public void PickupItem()
   {
      //格子超出限制
      if (!Inventory.instance.CanAdddItem() && itemData.itemType == ItemType.Equipment)
      {
         rb.linearVelocity = new Vector2(0, 7);
         return;
      }
      //如果玩家碰上--拾取到物品--添加到库存
      Inventory.instance.AddItem(itemData);
      Destroy(gameObject);
   }
}
