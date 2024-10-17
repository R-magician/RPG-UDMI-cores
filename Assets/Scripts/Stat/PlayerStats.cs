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
}
