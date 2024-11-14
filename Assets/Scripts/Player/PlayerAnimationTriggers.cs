//玩家动画触发器
using UnityEngine;

public class PlayerAnimationTriggers : MonoBehaviour
{
    //定义了一个只读变量--获取父组件上的Player主键
    private Player player => GetComponentInParent<Player>();
    
    private void AnimationTrigger()
    {
        //调用player上的触发器
        player.AnimationTrigger();
    }

    //攻击触发器
    private void AttackTrigger()
    {
        AudioManager.instance.PlaySFX(2);
        //创建一个圆形检测，获取所在范围的碰撞-这将只存在一帧
        Collider2D[] colliders = Physics2D.OverlapCircleAll(player.attackCheck.position, player.attackCheckRadius);

        foreach (var hit in colliders)
        {
            //如果检测到敌人
            if (hit.GetComponent<Enemy>() != null)
            {
                //获取敌人身上的统计
                EnemyStats _target = hit.GetComponent<EnemyStats>();

                if (_target != null)
                {
                    //对敌人造成伤害
                    player.stats.DoDamage(_target);
                }
                
                //库存获得武器，调用物品效果
                WeaponEffect(_target);
            }
        }
    }

    private void WeaponEffect(EnemyStats _target)
    {
        ItemDataEquipment weaponData = Inventory.instance.GetEquipment(EquipmentType.Weapon);

        weaponData?.Effect(_target.transform);
    }

    //飞剑技能
    private void ThrowSword()
    {
        SkillManager.instance.sword.CreateSword();
    }
}
 