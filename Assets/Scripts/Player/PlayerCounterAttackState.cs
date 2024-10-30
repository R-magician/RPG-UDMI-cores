//玩家反击状态
using UnityEngine;

public class PlayerCounterAttackState : PlayerState
{
    //能创建克隆体
    private bool canCreateClone;
    
    public PlayerCounterAttackState(Player _player, PlayerStateMachine _stateMachine, string _animeBoolName) : base(_player, _stateMachine, _animeBoolName)
    {
    }

    public override void Enter()
    {
        base.Enter();

        canCreateClone = true;
        //设置反击时间
        stateTimer = player.counterAttackDuration;
        //关闭成功反击动画
        player.anim.SetBool("SuccessfulCounterAttack",false);
    }

    public override void Update()
    {
        base.Update();
        
        //归0速度
        player.ZeroVelocity();
        
        //创建一个圆形检测，获取所在范围的碰撞-这将只存在一帧
        Collider2D[] colliders = Physics2D.OverlapCircleAll(player.attackCheck.position, player.attackCheckRadius);

        foreach (var hit in colliders)
        {
            //如果检测到敌人--与敌人交互
            if (hit.GetComponent<Enemy>() != null)
            {
                //可以站立
                if (hit.GetComponent<Enemy>().CanBeStunned())
                {
                    stateTimer = 10;//任意值，大于1
                    //开启成功反击动画
                    player.anim.SetBool("SuccessfulCounterAttack",true);
                    
                    //在格挡时恢复健康
                    player.skill.parry.UseSkill();

                    if (canCreateClone)
                    {
                        canCreateClone = false;
                        //反击成功创建一个克隆体
                        player.skill.parry.MakeMirageOnParry(hit.transform);
                    }
                }
            }
        }

        //过了反击时间 或者 触发器被调用
        if (stateTimer < 0 || triggerCalled)
        {
            //开启空闲状态
            stateMachine.ChangeState(player.playerIdleState);
        }
    }

    public override void Exit()
    {
        base.Exit();
    }
}
