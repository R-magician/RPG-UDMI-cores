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
        //播放受伤动画
        player.DamageEffect();
    }
}
