//在墙上跳跃
using UnityEngine;

public class PlayerWallJumpState : PlayerState
{
    public PlayerWallJumpState(Player _player, PlayerStateMachine _stateMachine, string _animeBoolName) : base(_player, _stateMachine, _animeBoolName)
    {
    }


    public override void Enter()
    {
        base.Enter();
        
        //状态定时器
        stateTime = 1f;

        //反方向调整冲刺方向
        player.dashDir = -player.dashDir;
        //给玩家提供速度--面对墙向反的方向
        player.SetVelocity(5 * -player.facingDir,player.jumpForce);
    }

    public override void Update()
    {
        base.Update();

        if (stateTime < 0)
        {
            //超过时间变成空中状态
            stateMachine.ChangeState(player.playerAirState);
        }

        //跳下后如果检测到地面就切换到idle状态
        if (player.IsGroundDetected())
        {
            stateMachine.ChangeState(player.playerIdleState);
        }
    }

    public override void Exit()
    {
        base.Exit();
        player.SetZeroVelocityX();
    }
}
