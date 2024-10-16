//物品特效
using UnityEngine;

[CreateAssetMenu(fileName = "新物品数据",menuName = "数据/物品特效")]
public class ItemEffect : ScriptableObject
{
    //实现效果
    public virtual void ExecuteEffect(Transform _respawnPosition)
    {
        Debug.Log("执行特效");
    }
}
