//人物数值统计计算
using UnityEngine;

public class CharacterStats : MonoBehaviour
{
    [Header("主要统计")]
    //攻击力--可以增加伤害1，重击率增加1%
    public Stat strength;
    //敏捷--提供闪避1%，运气1%
    public Stat agility;
    //智力--魔法伤害1，魔法抵抗力3
    public Stat intelligence;
    //活力--HP增加3-5点
    public Stat vitality;
    
    [Header("攻击统计")]
    //基础伤害
    public Stat damage;
    //基础暴击率
    public Stat critChance;
    //基础暴击伤害--默认值150%
    public Stat critPower;
    
    [Header("防御性统计")]
    //血量
    public Stat maxHealth;
    //基础护甲
    public Stat armor;
    //基础闪避
    public Stat evasion;
    //魔法抗性
    public Stat magicResistance;
    
    [Header("魔法统计")]
    //魔法伤害
    public Stat fireDamage;
    //冰雪伤害
    public Stat iceDamage;
    //雷电伤害
    public Stat lightningDamage;

    //是否被点燃
    public bool isIgnited;
    //是否是冰冷
    public bool isChilled;
    //是否被电
    public bool isShocked;
    
    //当前血量
    [SerializeField]private int currentHealth;

    protected virtual void Awake()
    {
        critPower.SetDefaultValue(150);
        currentHealth = maxHealth.GetValue();
    }

    //造成伤害--目标数据
    public virtual void DoDamage(CharacterStats _targetStats)
    {
        //闪避攻击判断
        if ( TargetCanAvoidAttack(_targetStats)) return;

        //总伤害=基础伤害+攻击力
        int totalDamage = strength.GetValue() + damage.GetValue();

        //暴击几率
        if (CanCrit())
        {
            //能暴击--计算暴击伤害
            totalDamage = CalculateCriticalDamage(totalDamage);
        }
        
        //检查攻击伤害
        totalDamage = CheckTargetArmor(_targetStats, totalDamage);

        //_targetStats.TakeDamage(totalDamage);
    }

    //魔法攻击
    public virtual void DoMagicalDamage(CharacterStats _targetStats)
    {
        int _fireDamage = fireDamage.GetValue();
        int _iceDamage = iceDamage.GetValue();
        int _lightningDamage = lightningDamage.GetValue();
        
        // 魔法伤害 + 智力
        int totalMagicalDamage= _fireDamage + _iceDamage + _lightningDamage + intelligence.GetValue();
        //检查目标抵抗力
        totalMagicalDamage = CheckTargetResistance(_targetStats, totalMagicalDamage);
        //目标受伤
        _targetStats.TakeDamage(totalMagicalDamage);
    }

    //检查目标抵抗力
    private int CheckTargetResistance(CharacterStats _targetStats, int totalMagicalDamage)
    {
        //魔法伤害 - 目标魔法抗性
        totalMagicalDamage -= (_targetStats.magicResistance.GetValue() + (_targetStats.intelligence.GetValue() * 3));
        //返回0--伤害中间的一个值
        totalMagicalDamage = Mathf.Clamp(totalMagicalDamage,0,int.MaxValue);
        return totalMagicalDamage;
    }

    //应用
    public void ApplyAilments(bool _ignite, bool _chill, bool _shock)
    {
        if (isChilled || isIgnited || isShocked)
        {
            return;
        }
        isIgnited = _ignite;
        isShocked = _shock;
        isChilled = _chill;
    }

    //受到伤害
    public virtual void TakeDamage(int _damage)
    {
        currentHealth -= _damage;
        if (currentHealth <= 0)
        {
            Die();
        }
    }

    //执行死亡
    protected virtual void Die()
    {
        
    }
    
    //可以闪避攻击
    private bool TargetCanAvoidAttack(CharacterStats _targetStats)
    {
        //闪避总值
        int totalEvasion = _targetStats.evasion.GetValue() + _targetStats.agility.GetValue();

        if (Random.Range(0, 100) < totalEvasion)
        {
            Debug.Log("闪避");
            return true;
        }

        return false;
    }
    
    //检查攻击伤害
    private int CheckTargetArmor(CharacterStats _targetStats, int totalDamage)
    {
        //总伤害减去护甲伤害
        totalDamage -= _targetStats.armor.GetValue();
        //把值限制在0以上
        totalDamage = Mathf.Clamp(totalDamage, 0, int.MaxValue);
        return totalDamage;
    }
    
    //暴击几率
    private bool CanCrit()
    {
        //几率，关键暴击率+敏捷度
        int totalCriticalChance = critChance.GetValue() + agility.GetValue();

        if (Random.Range(0, 100) <= totalCriticalChance)
        {
            return true;
        }
        return false;
    }

    //计算暴击伤害
    private int CalculateCriticalDamage(int _damage)
    {
        //暴击伤害=(基础暴击伤害+攻击力) * 0.01的几率
        float totalCritPower = (critPower.GetValue() + strength.GetValue()) * 0.01f;
        
        //伤害值 = 伤害*暴击伤害
        float critDamage = _damage * totalCritPower;
        
        //四舍五入
        return Mathf.RoundToInt(critDamage);
    }
}
