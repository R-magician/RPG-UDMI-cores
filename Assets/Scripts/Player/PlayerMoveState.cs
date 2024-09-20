//玩家移动状态
using UnityEngine;

public class PlayerMoveState : PlayerGroundedState
{
    
    
    public PlayerMoveState(Player _player, PlayerStateMachine _stateMachine, string _animeBoolName) : base(_player, _stateMachine, _animeBoolName)
    {
        //已经实现了父类的构造函数
    }

    //进入
    public override void Enter()
    {
        base.Enter();
    }

    //更新
    public override void Update()
    {
        base.Update();
        
        //获取冲刺方向
        if (inputDirection.x!=0)
        {
            player.dashDir = inputDirection.x;
        }
        
        //设置移动速度
        player.SetVelocity(inputDirection.x * player.moveSpeed,rb.linearVelocity.y);
        
        //切换移动动画--停止移动或者面向墙
        bool isMoving = Mathf.Abs(inputDirection.x) > 0;
        if (!isMoving || player.IsWallDetected())
        {
            stateMachine.ChangeState(player.playerIdleState);
        }
    }

    //退出
    public override void Exit()
    {
        base.Exit();
    }
}
