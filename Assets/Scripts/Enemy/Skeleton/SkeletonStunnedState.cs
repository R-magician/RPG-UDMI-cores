//骷髅眩晕状态
using UnityEngine;

public class SkeletonStunnedState : EnemyState
{
    private Enemy_Skeleton enemy;

    public SkeletonStunnedState(Enemy _enemyBase, EnemyStateMachine _stateMachine, string _animBoolName, Enemy_Skeleton enemy) : base(_enemyBase, _stateMachine, _animBoolName)
    {
        this.enemy = enemy;
    }

    public override void Enter()
    {
        base.Enter();

        //延迟0s，重复0.1f执行
        enemy.fx.InvokeRepeating("RedColorBlink",0,.1f);
        
        //眩晕时长
        stateTimer = enemy.stunDuration;
        //设置眩晕速度
        rb.linearVelocity = new (enemy.facingDir * enemy.stunDirection.x,enemy.stunDirection.y);
    }

    public override void Update()
    {
        base.Update();
        if (stateTimer < 0)
        {
            stateMachine.ChangeState(enemy.idleState);
        }
    }

    public override void Exit()
    {
        base.Exit();
        //立即执行关闭红色
        enemy.fx.Invoke("CancelRedBlink",0);
    }
}
