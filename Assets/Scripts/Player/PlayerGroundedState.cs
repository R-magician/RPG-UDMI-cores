//玩家地面状态(move，idle)需要被继承
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerGroundedState : PlayerState
{
    public PlayerGroundedState(Player _player, PlayerStateMachine _stateMachine, string _animeBoolName) : base(_player, _stateMachine, _animeBoolName)
    {
    }

    public override void Enter()
    {
        base.Enter();
    }

    public override void Update()
    {
        base.Update();
        player.inputControl.Player.Jump.started += Jump;
    }

    public override void Exit()
    {
        base.Exit();
    }
    
    //玩家跳跃方法
    private void Jump(InputAction.CallbackContext obj)
    {
        if (player.IsGroundDetected())
        {
            //碰撞体检测到地面才能起跳
            stateMachine.ChangeState(player.playerJumpState);
        }
    }
}
