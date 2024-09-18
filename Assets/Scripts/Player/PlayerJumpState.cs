//跳跃状态
using UnityEngine;

public class PlayerJumpState : PlayerState
{
    public PlayerJumpState(Player _player, PlayerStateMachine _stateMachine, string _animeBoolName) : base(_player, _stateMachine, _animeBoolName)
    {
    }


    public override void Enter()
    {
        base.Enter();
        //给y轴一个力
        rb.linearVelocity = new Vector2(rb.linearVelocity.x, player.jumpForce);
    }

    public override void Update()
    {
        base.Update();
        
        if (rb.linearVelocity.y < 0)
        {
            //当y轴的力小于0的时候-下降，改变状态
            stateMachine.ChangeState(player.playerAirState);
        }
    }

    public override void Exit()
    {
        base.Exit();
    }
}
