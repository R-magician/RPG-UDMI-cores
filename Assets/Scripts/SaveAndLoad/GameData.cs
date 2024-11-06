//游戏数据

using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class GameData
{
    public int currency;

    //库存列表(材质)
    public SerializableDictionary<string, int> inventory;

    public GameData()
    {
        this.currency = 0;
        inventory = new SerializableDictionary<string, int>();
    }
}
