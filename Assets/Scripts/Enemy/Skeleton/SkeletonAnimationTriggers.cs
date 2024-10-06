//玩家动画触发器
using UnityEngine;

public class SkeletonAnimationTriggers : MonoBehaviour
{
    //骷髅
    private Enemy_Skeleton enemy => GetComponentInParent<Enemy_Skeleton>();

    private void AnimationTrigger()
    {
        enemy.AnimationFinishTrigger();  
    }

    //攻击触发器
    private void AttackTrigger()
    {
        //创建一个圆形检测，获取所在范围的碰撞-这将只存在一帧
        Collider2D[] colliders = Physics2D.OverlapCircleAll(enemy.attackCheck.position, enemy.attackCheckRadius);

        foreach (var hit in colliders)
        {
            //如果检测到玩家
            if (hit.GetComponent<Player>() != null)
            {
                //获取player身上的统计
                PlayerStats target = hit.GetComponent<PlayerStats>();
                //对player造成伤害
                enemy.stats.DoDamage(target);
            }
        }
    }

    //触发反击
    private void OpenCounterWindow() => enemy.openCounterAttackWindow();

    //关闭反击
    private void CloseCounterWindow() => enemy.CloseCounterAttackWindow();
}
