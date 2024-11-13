//游戏数据

using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class GameData
{
    public int currency;

    //技能树
    public SerializableDictionary<string, bool> skillTree;
    //库存列表(材质)
    public SerializableDictionary<string, int> inventory;
    //存储列表
    public List<string> equipmentId;

    //检查点的存储
    public SerializableDictionary<string, bool> checkpoints;

    //最近的检查点
    public string closestCheckpointId;

    public GameData()
    {
        this.currency = 0;
        inventory = new SerializableDictionary<string, int>();
        equipmentId = new List<string>();
        skillTree = new SerializableDictionary<string, bool>();

        closestCheckpointId = string.Empty;
        checkpoints = new SerializableDictionary<string, bool>();
    }
}
