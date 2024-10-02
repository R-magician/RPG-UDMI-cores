//玩家掌握剑的状态
using UnityEngine;

public class PlayerCatchSwordState : PlayerState
{
    private Transform sword;
    
    public PlayerCatchSwordState(Player _player, PlayerStateMachine _stateMachine, string _animeBoolName) : base(_player, _stateMachine, _animeBoolName)
    {
    }

    public override void Enter()
    {
        base.Enter();

        sword = player.sword.transform;
        //判断鼠标在player的左边,并且玩家朝右
        if (player.transform.position.x > sword.position.x && player.facingDir ==1)
        {
            //翻转角色
            player.Flip();
        }else if (player.transform.position.x < sword.position.x && player.facingDir == -1)
        {
            player.Flip();
        }
        
        //收回剑的冲击力
        rb.linearVelocity = new Vector2(player.swordReturnImpact * -player.facingDir, rb.linearVelocity.y);
    }

    public override void Update()
    {
        base.Update();
        if (triggerCalled)
        {
            stateMachine.ChangeState(player.playerIdleState);
        }
    }

    public override void Exit()
    {
        base.Exit();
        //携程--让当前动画执行完毕
        player.StartCoroutine("BusyFor", .1f);
    }
}
