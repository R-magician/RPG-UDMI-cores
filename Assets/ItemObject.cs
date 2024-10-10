//物品对象

using System;
using UnityEngine;

public class ItemObject : MonoBehaviour
{
   private SpriteRenderer sr;
   //物品数据
   [SerializeField] private ItemData itemData;

   private void Awake()
   {
      sr = GetComponent<SpriteRenderer>();
      //设置图标
      sr.sprite = itemData.icon;
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
