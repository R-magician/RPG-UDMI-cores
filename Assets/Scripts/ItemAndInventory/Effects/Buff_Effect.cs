//buff效果
using UnityEngine;

[CreateAssetMenu(fileName = "Buff效果",menuName = "数据/物品特效/Buff效果")]
public class Buff_Effect : ItemEffect
{
    private PlayerStats stats;

    //buff类型
    [SerializeField] private StatType buffType;
    //数量
    [SerializeField] private int buffAmount;
    //持续时长
    [SerializeField] private float buffDuration;

    //效果
    public override void ExecuteEffect(Transform _respawnPosition)
    {
        stats = PlayerManager.instance.Player.GetComponent<PlayerStats>();
        stats.IncreaseStatBy(buffAmount,buffDuration,stats.GetStat(buffType));
    }
}
