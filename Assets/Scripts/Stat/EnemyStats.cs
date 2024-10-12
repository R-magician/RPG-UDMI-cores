//敌人伤害统计
using UnityEngine;

public class EnemyStats : CharacterStats
{
    private Enemy enemy;

    [Header("等级详情")]
    //等级
    [SerializeField] private int level=1;
    //修改器,数值范围--等级差值
    [Range(0f, 1f)] 
    [SerializeField] private float percantageModifier = .4f;
    
    protected override void Awake()
    {
        //应用等级增加的数值
        ApplyLevelModifiers();

        base.Awake();
        enemy = GetComponent<Enemy>();
    }

    //应用等级增加的数值
    private void ApplyLevelModifiers()
    {
        Modif(strength);
        Modif(agility);
        Modif(intelligence);
        Modif(vitality);
        
        Modif(damage);
        Modif(critChance);
        Modif(critPower);
        
        Modif(maxHealth);
        Modif(armor);
        Modif(evasion);
        Modif(magicResistance);
        
        Modif(fireDamage);
        Modif(iceDamage);
        Modif(lightningDamage);
    }

    //修改器
    private void Modif(Stat _stat)
    {
        for (int i = 1; i < level; i++)
        {
            float modifier = _stat.GetValue() * percantageModifier;
            _stat.AddModifier(Mathf.RoundToInt(modifier));
        }
    }
    
    public override void TakeDamage(int _damage)
    {
        base.TakeDamage(_damage);
    }

    protected override void Die()
    {
        base.Die();
        enemy.Die();
    }
}
