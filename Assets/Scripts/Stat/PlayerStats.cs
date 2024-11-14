using UnityEngine;

public class PlayerStats : CharacterStats
{
    private Player player;
    
    protected override void Awake()
    {
        base.Awake();
        player = GetComponent<Player>();
    }

    public override void TakeDamage(int _damage)
    {
        base.TakeDamage(_damage);
    }

    protected override void Die()
    {
        base.Die();
        //播放死亡动画
        player.Die();

        GameManager.instance.lostCurrencyAmount = PlayerManager.instance.currency;
        PlayerManager.instance.currency = 0;
        //掉落物品
        GetComponent<PlayerItemDrop>()?.GenerateDrop();
    }

    //减少生命值
    protected override void DecreaseHealthBy(int _damage)
    {
        base.DecreaseHealthBy(_damage);
        //获取当前的穿戴的盔甲
        ItemDataEquipment currentArmor = Inventory.instance.GetEquipment(EquipmentType.Armor);

        if (currentArmor != null)
        {
            //执行盔甲效果
            currentArmor.Effect(player.transform);
        }
    }

    //闪避时做的事情
    public override void OnEvasion()
    {
        player.skill.dodge.CreateMirageOnDoage();
    }

    //克隆的伤害
    public void CloneDoDamage(CharacterStats _targetStats,float _multiplier)
    {
        //闪避攻击判断
        if ( TargetCanAvoidAttack(_targetStats)) return;
        
        //总伤害=基础伤害+攻击力
        int totalDamage = strength.GetValue() + damage.GetValue();
        
        if (_multiplier > 0)
        {
            totalDamage = Mathf.RoundToInt(totalDamage * _multiplier);
        }

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
        DoMagicalDamage(_targetStats);//删除，如果你不想应用魔法作为主要攻击
    }
}
