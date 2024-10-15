//滑墙状态
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerWallSlideState : PlayerState
{
    public PlayerWallSlideState(Player _player, PlayerStateMachine _stateMachine, string _animeBoolName) : base(_player, _stateMachine, _animeBoolName)
    {
    }

    public override void Enter()
    {
        base.Enter();
        InputManager.instance.inputControl.Player.Jump.started += Jump;
    }

    public override void Update()
    {
        base.Update();
        
        //如果有输入
        if (inputDirection.x != 0)
        {
            //输入的方向和player面向方向相反
            if (player.facingDir != inputDirection.x)
            {
                //站立状态
                stateMachine.ChangeState(player.playerIdleState);
            }
        }

        //按了下键
        if (inputDirection.y <0)
        {
            //减缓下降的速度
            rb.linearVelocity = new Vector2(0f, rb.linearVelocity.y);
        }
        else
        {
            //减缓下降的速度
            rb.linearVelocity = new Vector2(0f, rb.linearVelocity.y * .7f);
        }
        
        //如果角色站立在地上
        if (player.IsGroundDetected())
        {
            //等待状态
            stateMachine.ChangeState(player.playerIdleState);
        }
    }

    public override void Exit()
    {
        base.Exit();
        InputManager.instance.inputControl.Player.Jump.started -= Jump;
    }
    
    //跳跃
    private void Jump(InputAction.CallbackContext obj)
    {
        //在墙上跳跃，切换到跳下墙状态--玩家没到地上
        if (player.IsWallDetected() && !player.IsGroundDetected())
        {
            //切换到墙跳状态
            stateMachine.ChangeState(player.playerWallJumpState);
        }
    }
}
