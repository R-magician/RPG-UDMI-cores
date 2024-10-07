//骨架死亡状态
using UnityEngine;

public class SkelectonDeadState : EnemyState
{
    //骨架敌人
    protected Enemy_Skeleton enemy;

    public SkelectonDeadState(Enemy _enemyBase, EnemyStateMachine _stateMachine, string _animBoolName, Enemy_Skeleton enemy) : base(_enemyBase, _stateMachine, _animBoolName)
    {
        this.enemy = enemy;
    }

    public override void Enter()
    {
        base.Enter();
        enemy.anim.SetBool(enemy.lastAnimBoolName, true);
        //敌人播放动画暂停
        enemy.anim.speed = 0;
        //敌人身上的碰撞器不可用
        enemy.cd.enabled = false;
        stateTimer = .1f;
    }

    public override void Update()
    {
        base.Update();
        if (stateTimer > 0)
        {
            //敌人跳一段向下掉
            rb.linearVelocity = new Vector2(0, 10);
        }
    }

    public override void Exit()
    {
        base.Exit();
    }
}
