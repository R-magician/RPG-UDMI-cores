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
        
        //创建一个克隆体
        player.skill.dash.CloneOnDash();
        
        //冲刺时间
        stateTimer = player.dashDuration;
        //无敌状态
        player.stats.MakeInvincible(true);
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
        
        if (stateTimer < 0)
        {
            //当冲刺时间结束，切换成等待状态
            stateMachine.ChangeState(player.playerIdleState);
        }
        
        player.fx.CreateAfterImage();
    }

    public override void Exit()
    {
        base.Exit();
        player.skill.dash.CloneOnArrival();
        //退出状态的时候重置一下速度
        player.SetVelocity(0,rb.linearVelocity.y);
        //无敌状态
        player.stats.MakeInvincible(false);
    }
}
