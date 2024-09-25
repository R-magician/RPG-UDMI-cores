//骷髅准备战斗状态
using UnityEngine;

public class SkeletonBattleState : EnemyState
{
    //获取到玩家
    private Transform player;
    //骨架敌人
    private Enemy_Skeleton enemy;
    //移动方向
    private int moveDir;
    
    public SkeletonBattleState(Enemy _enemyBase, EnemyStateMachine _stateMachine, string _animBoolName,Enemy_Skeleton _enemy) : base(_enemyBase, _stateMachine, _animBoolName)
    {
        enemy = _enemy;
    }


    public override void Enter()
    {
        base.Enter();

        player = PlayerManager.instance.Player.transform;
    }

    public override void Update()
    {
        base.Update();

        if (enemy.isPlayerDetected())
        {
            stateTimer = enemy.battleTime;
            //当检测距离小于骷髅攻击距离--停下，发动攻击
            if (enemy.isPlayerDetected().distance < enemy.attackDistance)
            {
                if (CanAttack())
                {
                    //切换成攻击状态
                    stateMachine.ChangeState(enemy.attackState);
                }
            }
        }
        else
        {
            //当战斗时间过了，恢复空闲状态,或者当玩家远离骷髅7个单位
            if (stateTimer < 0 || Vector2.Distance(player.transform.position,enemy.transform.position) > 7)
            {
                stateMachine.ChangeState(enemy.idleState);
            }
        }

        if (player.position.x > enemy.transform.position.x)
        {
            //骷髅在左边
            moveDir = 1;
        } 
        else if(player.position.x < enemy.transform.position.x)
        {
            //骷髅在右边
            moveDir = -1;
        }
        
        enemy.SetVelocity(enemy.moveSpeed * moveDir ,rb.linearVelocity.y);
    }

    public override void Exit()
    {
        base.Exit();
    }

    //是否可以攻击
    private bool CanAttack()
    {
        if (Time.time >= enemy.lastTimeAttacked + enemy.attackCooldown)
        {
            enemy.lastTimeAttacked = Time.time;
            return true;
        }
        return false;
    }
}
