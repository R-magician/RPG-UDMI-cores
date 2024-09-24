using UnityEngine;

public class SkeletonGroundedState : EnemyState
{
    //骨架敌人
    protected Enemy_Skeleton enemy;

    protected Transform player;

    public SkeletonGroundedState(Enemy _enemyBase, EnemyStateMachine _stateMachine, string _animBoolName, Enemy_Skeleton _enemy) : base(_enemyBase, _stateMachine, _animBoolName)
    {
        enemy = _enemy;
    }

    public override void Enter()
    {
        base.Enter();
        player = GameObject.Find("Player").transform;
    }

    public override void Update()
    {
        base.Update();

        //检测是否是玩家--或者骷髅和角色直接少于两个单位进入战斗状态
        if (enemy.isPlayerDetected() || Vector2.Distance(enemy.transform.position,player.position) < 2)
        {
            stateMachine.ChangeState(enemy.battleState);   
        }
    }

    public override void Exit()
    {
        base.Exit();
    }
}
