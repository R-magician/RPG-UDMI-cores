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
        //跳跃
        player.inputControl.Player.Jump.started += Jump;
        //冲刺
        player.inputControl.Player.Dash.started += Dash;
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
    
    //玩家冲刺
    private void Dash(InputAction.CallbackContext obj)
    {
        stateMachine.ChangeState(player.playerDashState);
    }
}
