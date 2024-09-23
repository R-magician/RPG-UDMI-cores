using UnityEngine;

public class SkeletonGroundedState : EnemyState
{
    //骨架敌人
    protected Enemy_Skeleton enemy;

    public SkeletonGroundedState(Enemy _enemyBase, EnemyStateMachine _stateMachine, string _animBoolName, Enemy_Skeleton _enemy) : base(_enemyBase, _stateMachine, _animBoolName)
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

        //检测是否是玩家
        if (enemy.isPlayerDetected())
        {
            stateMachine.ChangeState(enemy.battleState);   
        }
    }

    public override void Exit()
    {
        base.Exit();
    }
}
