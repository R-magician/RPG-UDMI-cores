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
        //瞄准状态开启点
        player.skill.sword.DotsActive(true);
    }
    

    public override void Update()
    {
        base.Update();
        
        //返回归0
        player.ZeroVelocity();
        
        //获取鼠标点击的坐标
        Vector2 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);

        //判断鼠标在player的左边,并且玩家朝右
        if (player.transform.position.x > mousePos.x && player.facingDir ==1)
        {
            //翻转角色
            player.Flip();
        }else if (player.transform.position.x < mousePos.x && player.facingDir == -1)
        {
            player.Flip();
        }
    }

    public override void Exit()
    {
        base.Exit();

        //携程--让当前动画执行完毕
        player.StartCoroutine("BusyFor", .2f);
    }
    
    //手里剑
    private void ViceSkill(InputAction.CallbackContext obj)
    {
        stateMachine.ChangeState(player.playerIdleState);
    }
}
