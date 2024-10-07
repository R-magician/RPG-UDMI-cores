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

        _targetStats.TakeDamage(totalDamage);
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
