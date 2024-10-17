//buff效果
using UnityEngine;

public enum StatType {
    strength,//攻击力
    agility,//敏捷
    intelligence,//智力
    vitality,//活力
    damage,//基础伤害
    critChance,//基础暴击率
    critPower,//基础暴击伤害
    maxHealth,//血量
    armor,//基础护甲
    evasion,//基础闪避
    magicResistance,//魔法抗性
    fireDamage,//火
    iceDamage,//冰
    lightningDamage//雷电伤害
}

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
        stats.IncreaseStatBy(buffAmount,buffDuration,StatToModify());
    }

    //获取buff对应的数值
    private Stat StatToModify()
    {
        if (buffType == StatType.strength)
        {
            return stats.strength;
        }else if (buffType == StatType.agility)
        {
            return stats.agility;
        }else if (buffType == StatType.intelligence)
        {
            return stats.intelligence;
        }else if (buffType == StatType.vitality)
        {
            return stats.vitality;
        }else if (buffType == StatType.damage)
        {
            return stats.damage;
        }else if (buffType == StatType.critChance)
        {
            return stats.critChance;
        }else if (buffType == StatType.critPower)
        {
            return stats.critPower;
        }else if (buffType == StatType.maxHealth)
        {
            return stats.maxHealth;
        }else if (buffType == StatType.armor)
        {
            return stats.armor;
        }else if (buffType == StatType.evasion)
        {
            return stats.evasion;
        }else if (buffType == StatType.magicResistance)
        {
            return stats.magicResistance;
        }else if (buffType == StatType.fireDamage)
        {
            return stats.fireDamage;
        }else if (buffType == StatType.iceDamage)
        {
            return stats.iceDamage;
        }else if (buffType == StatType.lightningDamage)
        {
            return stats.lightningDamage;
        }
        else
        {
            return null;
        }
    }
}
