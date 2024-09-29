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
        //跳跃
        player.inputControl.Player.Jump.started += Jump;
        //攻击
        player.inputControl.Player.Attack.started += Attack;
        //反击
        player.inputControl.Player.CounterAttack.started += CounterAttack;
        //手里剑--按键开始
        player.inputControl.Player.ViceSkill.started  += ViceSkill;
    }

    public override void Update()
    {
        base.Update();
        //移动
        Move();
        //当角色在空中的时候切换状态
        if (!player.IsGroundDetected())
        {
            stateMachine.ChangeState(player.playerAirState);
        }
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
    
    //玩家攻击
    private void Attack(InputAction.CallbackContext obj)
    {
        stateMachine.ChangeState(player.playerPrimaryAttack);
    }
    
    //玩家反击
    private void CounterAttack(InputAction.CallbackContext obj)
    {
        stateMachine.ChangeState(player.playerCounterAttackState);
    }
    
    //手里剑
    private void ViceSkill(InputAction.CallbackContext obj)
    {
        stateMachine.ChangeState(player.playerAimSwordState);
    }

    private void Move()
    {
        //切换移动动画
        bool isMoving = Mathf.Abs(inputDirection.x) > 0;
        
        //玩家在移动，但是没有碰到墙
        if (isMoving)
        {
            //isBusy--携程没有忙碌的时候可以移动
            if ((!player.IsWallDetected() || player.facingDir != inputDirection.x) && !player.isBusy)
            {
                stateMachine.ChangeState(player.playerMoveState);
            }
        }
    }
}
