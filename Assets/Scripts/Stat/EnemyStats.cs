using UnityEngine;

public class EnemyStats : CharacterStats
{
    private Enemy enemy;
    protected override void Awake()
    {
        base.Awake();
        enemy = GetComponent<Enemy>();
    }
    
    public override void TakeDamage(int _damage)
    {
        base.TakeDamage(_damage);
        //播放受伤动画
        enemy.DamageEffect();
    }
}
