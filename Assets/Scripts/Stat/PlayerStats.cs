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
}
