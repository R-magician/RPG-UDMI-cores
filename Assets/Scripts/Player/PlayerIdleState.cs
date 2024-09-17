//玩家等待状态
using UnityEngine;

public class PlayerIdleState :PlayerState
{
    public PlayerIdleState(Player _player, PlayerStateMachine _stateMachine, string _animeBoolName) : base(_player, _stateMachine, _animeBoolName)
    {
        //已经实现了父类的构造函数
    }

    //进入
    public override void Enter()
    {
        base.Enter();
    }

    //更新
    public override void Update()
    {
        base.Update();
        
        //切换移动动画
        bool isMoving = Mathf.Abs(inputDirection.x) > 0;
        if (isMoving)
        {
            stateMachine.ChangeState(player.playerMoveState);
        }
    }
    
    //退出
    public override void Exit()
    {
        base.Exit();
    }
}
