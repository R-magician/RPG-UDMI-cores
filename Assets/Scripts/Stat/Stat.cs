//数值处理

using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Stat 
{
    //一个状态的基础值
    [SerializeField] private int baseValue;

    //修改器--伤害值列表，当玩家有buff时添加在列表中进行计算
    public List<int> modifiers;

    public int GetValue()
    {
        int finalValue = baseValue;
        foreach (int modifier in modifiers)
        {
            finalValue += modifier;
        }
        return finalValue;
    }

    //添加buff数值
    public void AddModifier(int _modifier)
    {
        modifiers.Add(_modifier);
    }
    
    //移除buff数值
    public void removeModifier(int index)
    {
        modifiers.RemoveAt(index);
    }
}
