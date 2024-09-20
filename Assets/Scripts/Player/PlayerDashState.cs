//玩家冲刺状态
using UnityEngine;

public class PlayerDashState : PlayerState
{
    public PlayerDashState(Player _player, PlayerStateMachine _stateMachine, string _animeBoolName) : base(_player, _stateMachine, _animeBoolName)
    {
    }

    public override void Enter()
    {
        base.Enter();
        //冲刺时间
        stateTime = player.dashDuration;
    }

    public override void Update()
    {
        base.Update();

        //如果没有在地上，检测到了墙
        if (!player.IsGroundDetected() && player.IsWallDetected())
        {
            //切换状态
            stateMachine.ChangeState(player.playerWallSlideState);
        }
        
        //设置玩家冲刺
        player.SetVelocity(player.dashSpeed * player.dashDir,0);
        
        if (stateTime < 0)
        {
            //当冲刺时间结束，切换成等待状态
            stateMachine.ChangeState(player.playerIdleState);
        }
    }

    public override void Exit()
    {
        base.Exit();
        //退出状态的时候重置一下速度
        player.SetVelocity(0,rb.linearVelocity.y);
    }
}
