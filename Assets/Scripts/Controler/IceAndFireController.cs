//冰火控制器
using UnityEngine;

public class IceAndFireController : ThunderStrikeController
{
   protected override void OnTriggerEnter2D(Collider2D other)
   {
      base.OnTriggerEnter2D(other);
   }
}