//骨架移动状态
using UnityEngine;

public class SkeletonMoveState : EnemyState
{
    private Enemy_Skeleton enemy;
    
    public SkeletonMoveState(Enemy _enemyBase, EnemyStateMachine _stateMachine, string _animBoolName,Enemy_Skeleton _enemy) : base(_enemyBase, _stateMachine, _animBoolName)
    {
        enemy = _enemy;
    }
    
    public override void Enter()
    {
        base.Enter();
    }

    public override void Update()
    {
        base.Update();
        //设置速度
        enemy.SetVelocity(enemy.moveSpeed * enemy.facingDir,enemy.rb.linearVelocity.y);
        
        if (enemy.IsWallDetected() || !enemy.IsGroundDetected())
        {
            enemy.Flip();
            stateMachine.ChangeState(enemy.idleState);
        }
        
    }

    public override void Exit()
    {
        base.Exit();
    }
}
