//游戏数据

using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class GameData
{
    public int currency;

    //库存列表(材质)
    public SerializableDictionary<string, int> inventory;
    //存储列表
    public List<string> equipmentId;

    public GameData()
    {
        this.currency = 0;
        inventory = new SerializableDictionary<string, int>();
        equipmentId = new List<string>();
    }
}
