//敌人伤害统计
using UnityEngine;

public class EnemyStats : CharacterStats
{
    private Enemy enemy;
    //掉落物品
    private ItemDrop myDropSystem;
    //灵魂的掉落数
    public Stat soulsDropAmount;

    [Header("等级详情")]
    //等级
    [SerializeField] private int level=1;
    //修改器,数值范围--等级差值
    [Range(0f, 1f)] 
    [SerializeField] private float percantageModifier = .4f;
    
    protected override void Awake()
    {
        soulsDropAmount.SetDefaultValue(100);
        //应用等级增加的数值
        ApplyLevelModifiers();

        base.Awake();
        enemy = GetComponent<Enemy>();
        myDropSystem = GetComponent<ItemDrop>();
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
        
        Modif(soulsDropAmount);
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
        //玩家自身增加灵魂
        PlayerManager.instance.currency += soulsDropAmount.GetValue();
        myDropSystem.GenerateDrop();
        
        //删除敌人
        Destroy(gameObject,3f);
    }
}
