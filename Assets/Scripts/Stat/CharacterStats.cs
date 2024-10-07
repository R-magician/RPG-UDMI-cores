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

    [Header("状态反馈")]
    //是否被点燃--随着时间的推移造成伤害
    public bool isIgnited;
    //是否是冰冷--减缓速度
    public bool isChilled;
    //是否被电--增加后续伤害
    public bool isShocked;

    //点燃计时器
    private float ignitedTimer;
    //冰冻计时器
    private float chilledTimer;
    //电击计时器
    private float shockTimer;
    
    //点燃冷却时间
    private float ignitedDamageCooldown = .3f;
    //冰冻冷却时间
    private float chilledDamageCooldown = .3f;
    //电击冷却时间
    private float shockedDamageCooldown = .3f;
    //点燃伤害计时器
    private float ignitedDamageTimer;
    //冰冻伤害计时器
    private float chilledDamageTimer;
    //电击伤害计时器
    private float shockedDamageTimer;
    //点燃伤害
    private int igniteDamage;
    
    
    //当前血量
    [SerializeField]private int currentHealth;

    protected virtual void Awake()
    {
        critPower.SetDefaultValue(150);
        currentHealth = maxHealth.GetValue();
    }

    protected virtual void Update()
    {
        //点燃计时器
        ignitedTimer -= Time.deltaTime;
        chilledTimer -= Time.deltaTime;
        shockTimer -= Time.deltaTime;
        
        //点燃伤害计时器
        ignitedDamageTimer -= Time.deltaTime;
        chilledDamageTimer -= Time.deltaTime;
        shockedDamageTimer -= Time.deltaTime;

        if (ignitedTimer < 0)
        {
            //点燃结束
            isIgnited = false;
            if (ignitedDamageTimer < 0)
            {
                //点燃伤害结束
                TakeDamage(igniteDamage);
                
                ignitedDamageTimer = ignitedDamageCooldown;
            }
        }
        
        if (chilledTimer < 0)
        {
            //冰冻结束
            isChilled = false;
            if (chilledDamageTimer < 0)
            {
                //冰冻伤害结束
                chilledDamageTimer = chilledDamageCooldown;
            }
        }
        
        if (shockTimer < 0)
        {
            //电击结束
            isShocked = false;
            if (shockedDamageTimer < 0)
            {
                //电击伤害结束
                shockedDamageTimer = shockedDamageCooldown;
            }
        }
        
        
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
        DoMagicalDamage(_targetStats);
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

        if (Mathf.Max(_fireDamage, _iceDamage, _lightningDamage) <= 0)
        {
            //当没有魔法伤害退出
            return;
        }
        
        //能被点燃--火的伤害大于冰，并且火的伤害大于雷
        bool canApplyIgnite = _fireDamage > _iceDamage && _fireDamage > _lightningDamage;
        //能被冰冻--冰的伤害大于火，并且冰的伤害大于雷
        bool canApplyChill = _iceDamage > _fireDamage && _iceDamage > _lightningDamage;
        //能被点击--雷的伤害大于火，并且雷的伤害大于冰
        bool canApplyShock = _lightningDamage > _fireDamage && _lightningDamage > _iceDamage;

        while (!canApplyIgnite && !canApplyChill && !canApplyShock)
        {
            //当所以都不满足时--Random.value--(0,1)
            if (Random.value < .5f && _fireDamage > 0)
            {
                canApplyIgnite = true;
                _targetStats.ApplyAilments(canApplyIgnite, canApplyChill, canApplyShock);
                return;
            }
            
            if (Random.value < .5f && _iceDamage > 0)
            {
                canApplyChill = true;
                _targetStats.ApplyAilments(canApplyIgnite, canApplyChill, canApplyShock);
                return;
            }
            
            if (Random.value < .5f && _lightningDamage > 0)
            {
                canApplyShock = true;
                _targetStats.ApplyAilments(canApplyIgnite, canApplyChill, canApplyShock);
                return;
            }
        }

        if (canApplyIgnite)
        {
            //设置伤害，点燃的20%
            _targetStats.SetupIgniteDamage(Mathf.RoundToInt(_fireDamage * .2f));
        }
        
        //状态应用
        _targetStats.ApplyAilments(canApplyIgnite, canApplyChill, canApplyShock);
    }

    //检查目标抵抗力
    private int CheckTargetResistance(CharacterStats _targetStats, int totalMagicalDamage)
    {
        //如果目标是冰冻的
        if (_targetStats.isChilled)
        {
            totalMagicalDamage -= Mathf.RoundToInt(_targetStats.armor.GetValue() * .8f);
        }
        else
        {
            //魔法伤害 - 目标魔法抗性
            totalMagicalDamage -= (_targetStats.magicResistance.GetValue() + (_targetStats.intelligence.GetValue() * 3));
        }
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

        if (_ignite)
        {
            //点燃
            isIgnited = _ignite;
            ignitedTimer = 2;
        }

        if (_chill)
        {
            //冰冻
            isChilled = _chill;
            chilledTimer = 2;
        }

        if (_shock)
        {
            //电击
            isShocked = _shock;
            shockTimer = 2;
        }
        
       
    }

    //设置点燃伤害
    public void SetupIgniteDamage(int _damage)
    {
        igniteDamage = _damage;
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
    
    //目标可以闪避攻击
    private bool TargetCanAvoidAttack(CharacterStats _targetStats)
    {
        //闪避总值
        int totalEvasion = _targetStats.evasion.GetValue() + _targetStats.agility.GetValue();

        //被电
        if (isShocked)
        {
            //目标闪避+20
            totalEvasion += 20;
        }
        
        if (Random.Range(0, 100) < totalEvasion)
        {
            Debug.Log("闪避");
            return true;
        }

        return false;
    }
    
    //检查目标攻击伤害
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
