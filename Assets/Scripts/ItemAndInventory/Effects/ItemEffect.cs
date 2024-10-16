//物品特效
using UnityEngine;

public class ItemEffect : ScriptableObject
{
    //实现效果
    public virtual void ExecuteEffect(Transform _respawnPosition)
    {
        Debug.Log("执行特效");
    }
}
