//人物数值统计计算
using UnityEngine;

public class CharacterStats : MonoBehaviour
{
    //特效
    private EnityFX fx;
    
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
    
    //Debuff持续时间
    [SerializeField] private float alimentsDuration = 4;
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
    //点燃时候持续伤害
    private int igniteDamage;
    //电击时候持续伤害
    private int shockDamage;
    //雷电预制体
    [SerializeField] private GameObject shockStrikePrefab;
    
    //注册事件--血量更新
    public System.Action onHealthChanged;
    //死亡状态
    public bool isDead { get; private set; }
    
    
    //当前血量
    public int currentHealth;

    protected virtual void Awake()
    {
        critPower.SetDefaultValue(150);
        //当前生命值
        currentHealth = GetMaxHealthValue();
        
        fx = GetComponent<EnityFX>();
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
        }
        
        if (chilledTimer < 0)
        {
            //冰冻结束
            isChilled = false;
            chilledTimer = chilledDamageCooldown;
            
        }
        
        if (shockTimer < 0)
        {
            //电击结束
            isShocked = false;
            shockedDamageTimer = shockedDamageCooldown;
        }

        if (isIgnited)
        {
            ApplyIgniteDamage();
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

        _targetStats.TakeDamage(totalDamage);
        //如果有属性伤害就能造成魔法攻击
        //DoMagicalDamage(_targetStats);
    }

    #region 魔法伤害

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
        //尝试应用元素
        AttemptyToApplyAilements(_targetStats, _fireDamage, _iceDamage, _lightningDamage);
    }

    //尝试应用元素
    private void AttemptyToApplyAilements(CharacterStats _targetStats, int _fireDamage, int _iceDamage,
        int _lightningDamage)
    {
        //能被点燃--火的伤害大于冰，并且火的伤害大于雷
        bool canApplyIgnite = _fireDamage > _iceDamage && _fireDamage > _lightningDamage;
        //能被冰冻--冰的伤害大于火，并且冰的伤害大于雷
        bool canApplyChill = _iceDamage > _fireDamage && _iceDamage > _lightningDamage;
        //能被点击--雷的伤害大于火，并且雷的伤害大于冰
        bool canApplyShock = _lightningDamage > _fireDamage && _lightningDamage > _iceDamage;

        while (!canApplyIgnite && !canApplyChill && !canApplyShock)
        {
            //当所以都不满足时--Random.value--(0,1)--概率百分之50触发
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
            //设置持续伤害，点燃的20%
            _targetStats.SetupIgniteDamage(Mathf.RoundToInt(_fireDamage * .2f));
        }
        
        if (canApplyShock)
        {
            //设置持续伤害，电击的20%
            _targetStats.SetupShockDamage(Mathf.RoundToInt(_iceDamage * .1f));
        }
        
        //状态应用
        _targetStats.ApplyAilments(canApplyIgnite, canApplyChill, canApplyShock);
    }

    //应用
    public void ApplyAilments(bool _ignite, bool _chill, bool _shock)
    {
        //可以被点燃
        bool canApplyIgnite = !isIgnited && !isChilled && !isShocked;
        //可以冰冻
        bool canApplyChill = !isIgnited && !isChilled && !isShocked;
        //可以电击
        bool canApplyShock = !isIgnited && !isChilled;

        if (_ignite && canApplyIgnite)
        {
            //点燃
            isIgnited = _ignite;
            ignitedTimer = alimentsDuration;
            //点燃特效
            fx.IgniteFxFor(alimentsDuration);
        }

        if (_chill && canApplyChill)
        {
            //冰冻
            isChilled = _chill;
            chilledTimer = alimentsDuration;

            float slowPercentage = .2f;
            //减速0.2s
            GetComponent<Enity>().SlowEntityBy(slowPercentage,alimentsDuration);
            //冰冻特效
            fx.ChillFxFor(alimentsDuration);
        }

        //电击可以传染
        if (_shock && canApplyShock)
        {
            if (!isShocked)
            {
                //应用闪电持续伤害
                ApplyShock(_shock);
            }
            else
            {
                //当前对象是player 直接退出-不传递电击
                if (GetComponent<Player>() != null)
                {
                    return;
                }
                //攻击最近的目标--只对敌人有效
                HitNearestTargetWith();
            }
        }
    }

    //应用闪电持续伤害
    public void ApplyShock(bool _shock)
    {
        //如果已经被电了，返回
        if (isShocked)
        {
            return;
        }
        //电击
        isShocked = _shock;
        shockTimer = alimentsDuration;
            
        //雷电特效
        fx.ShockFxFor(alimentsDuration);
    }

    //攻击最近的目标--只对敌人有效
    private void HitNearestTargetWith()
    {
        //找到最近的目标，只在敌人中寻找
        Collider2D[] colliders = Physics2D.OverlapCircleAll(transform.position, 25f);
        
        //默认等于无限大
        float closestDistance = Mathf.Infinity;
        
        Transform closestEnemy = null;

        foreach (var hit in colliders)
        {
            //防止获取的对象是当前 -- Vector2.Distance(transform.position,hit.transform.position)>1
            if (hit.GetComponent<Enemy>() != null && Vector2.Distance(transform.position,hit.transform.position)>1)
            {
                //区域范围中用敌人--获取之间相隔的距离
                float distanceToEnemy = Vector2.Distance(transform.position, hit.transform.position);
                if (distanceToEnemy < closestDistance)
                {
                    //找到最近的那个敌人
                    closestDistance = distanceToEnemy;
                    closestEnemy = hit.transform;
                }
            }

            if (closestEnemy == null)
            {
                closestEnemy = transform;
            }
        }

        if (closestEnemy != null)
        {
            //生成预制体
            GameObject newShockStrike = Instantiate(shockStrikePrefab, transform.position, Quaternion.identity);
            //设置伤害和数据统计
            newShockStrike.GetComponent<ShockStrikeController>().Setup(shockDamage,closestEnemy.GetComponent<CharacterStats>());
        }
    }

    //应用点火伤害
    private void ApplyIgniteDamage()
    {
        if (ignitedDamageTimer < 0)
        {
            //点燃伤害结束 
            TakeDamage(igniteDamage);
                
            ignitedDamageTimer = ignitedDamageCooldown;
        }
    }
    
    //设置点燃持续伤害
    public void SetupIgniteDamage(int _damage)
    {
        igniteDamage = _damage;
    }
    
    //设置电击持续伤害
    public void SetupShockDamage(int _damage)
    {
        shockDamage = _damage;
    }
    
    #endregion

    //受到伤害
    public virtual void TakeDamage(int _damage)
    {
        //减少生命值
        DecreaseHealthBy(_damage);
        
        GetComponent<Enity>().DamageImpact();
        fx.StartCoroutine("FlashFx");
        
        if (currentHealth <= 0 && !isDead)
        {
            Die();
        }
    }

    //增加血量
    public virtual void IncreaseHealthBy(int _amount)
    {
        currentHealth += _amount;
        if (currentHealth > GetMaxHealthValue())
        {
            currentHealth = GetMaxHealthValue();
        }

        if (onHealthChanged != null)
        {
            //血量更新UI
            onHealthChanged();
        }
    }

    //减少生命值
    protected virtual void DecreaseHealthBy(int _damage)
    {
        currentHealth -= _damage;
        
        if (onHealthChanged!=null)
        {
            //更新UI
            onHealthChanged();
        }
    }

    //执行死亡
    protected virtual void Die()
    {
        isDead = true;
    }

    #region 计算区域
    
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

    //获取当前最大生命值
    public int GetMaxHealthValue()
    {
        //当前血量=最大生命值+活力
        return maxHealth.GetValue() + vitality.GetValue() * 5;
    }
    
    #endregion
}
