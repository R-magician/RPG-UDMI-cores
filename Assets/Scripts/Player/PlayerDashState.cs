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
        
        //设置玩家冲刺
        player.SetVelocity(player.dashSpeed * player.facingDir,rb.linearVelocity.y);
        
        if (stateTime < 0)
        {
            //当冲刺时间结束，切换成等待状态
            stateMachine.ChangeState(player.playerIdleState);
        }
    }

    public override void Exit()
    {
        base.Exit();
    }
}
