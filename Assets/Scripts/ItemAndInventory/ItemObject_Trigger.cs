using UnityEngine;

public class ItemObject_Trigger : MonoBehaviour
{
    private ItemObject myItemObject => GetComponentInParent<ItemObject>();
    
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.GetComponent<Player>() != null)
        {
            //拾取物品
            myItemObject.PickupItem();
        }
    }
}
