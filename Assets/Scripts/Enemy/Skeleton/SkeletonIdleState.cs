//骨架空闲状态
using UnityEngine;

public class SkeletonIdleState : EnemyState
{
    //骨架敌人
    private Enemy_Skeleton enemy;
    
    public SkeletonIdleState(Enemy _enemyBase, EnemyStateMachine _stateMachine, string _animBoolName,Enemy_Skeleton _enemy) : base(_enemyBase, _stateMachine, _animBoolName)
    {
        enemy = _enemy;
    }

    public override void Enter()
    {
        base.Enter();
        //状态计时器--敌人等待时间
        stateTimer = enemy.idleTime;
    }

    public override void Update()
    {
        base.Update();
        if (stateTimer < 0)
        {
            stateMachine.ChangeState(enemy.moveState);
        }
        
    }

    public override void Exit()
    {
        base.Exit();
    }
}
