//玩家瞄准剑状态
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerAimSwordState : PlayerState
{
    public PlayerAimSwordState(Player _player, PlayerStateMachine _stateMachine, string _animeBoolName) : base(_player, _stateMachine, _animeBoolName)
    {
    }

    public override void Enter()
    {
        base.Enter();
        //手里剑--按键结束
        player.inputControl.Player.ViceSkill.canceled += ViceSkill;
    }
    

    public override void Update()
    {
        base.Update();
    }

    public override void Exit()
    {
        base.Exit();
    }
    
    //手里剑
    private void ViceSkill(InputAction.CallbackContext obj)
    {
        stateMachine.ChangeState(player.playerIdleState);
    }
}
