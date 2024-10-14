using UnityEngine;

public class ItemObject_Trigger : MonoBehaviour
{
    private ItemObject myItemObject => GetComponentInParent<ItemObject>();
    
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.GetComponent<Player>() != null)
        {
            if (other.GetComponent<CharacterStats>().isDead)
            {
                //如果玩家死亡直接退出
                return;
            }
            //拾取物品
            myItemObject.PickupItem();
        }
    }
}
