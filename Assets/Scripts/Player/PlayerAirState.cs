//玩家停留空中状态
using UnityEngine;

public class PlayerAirState : PlayerState
{
    public PlayerAirState(Player _player, PlayerStateMachine _stateMachine, string _animeBoolName) : base(_player, _stateMachine, _animeBoolName)
    {
    }

    public override void Enter()
    {
        base.Enter();
    }

    public override void Update()
    {
        base.Update();

        //在墙上--移动方向和面朝方向一致
        if (player.IsWallDetected() && player.facingDir == (int)inputDirection.x)
        {
            stateMachine.ChangeState(player.playerWallSlideState);
        }
        
        //在地上
        if (player.IsGroundDetected())
        {
            //player在地面的时候，改变为等待状态
            stateMachine.ChangeState(player.playerIdleState);
        }
        
        //当角色在空中靠墙
        if (inputDirection.x != 0)
        {
            player.SetVelocity(player.moveSpeed * .8f * inputDirection.x,rb.linearVelocity.y);   
        }
    }

    public override void Exit()
    {
        base.Exit();
    }
}
