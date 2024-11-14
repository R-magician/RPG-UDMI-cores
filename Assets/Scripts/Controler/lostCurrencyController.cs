//玩家丢失货币

using System;
using UnityEngine;

public class lostCurrencyController : MonoBehaviour
{
    //货币数量
    public int currency;

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.GetComponent<Player>() != null)
        {
            //获得货币
            PlayerManager.instance.currency += currency;
            Destroy(this.gameObject);
        }
    }
}
