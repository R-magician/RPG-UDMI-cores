//玩家管理器

using System;
using UnityEngine;
using UnityEngine.Serialization;

[DefaultExecutionOrder(-100)]
public class PlayerManager : MonoBehaviour,ISaveManager
{
    //单例模式
    public static PlayerManager instance;

    //玩家
    public Player Player;

    //当前货币
    [FormerlySerializedAs("current")] public int currency;

    private void Awake()
    {
        //只有一个实例，若有了销毁 
        if (instance != null)
        {
            Destroy(instance.gameObject);
        }
        else
        {
            instance = this;
        }
    }

    //是否有足够的货币
    public bool HaveEnoughMoney(int _price)
    {
        if (_price > currency)
        {
            //没有足够的货币
            return false;
        }
        currency -= _price;
        return true;
    }

    //返回数量
    public int GetCurrency()
    {
        return currency;
    }

    public void LoadData(GameData _data)
    {
        this.currency = _data.currency;
    }

    public void SaveData(ref GameData _data)
    {
        _data.currency = this.currency;
    }
}