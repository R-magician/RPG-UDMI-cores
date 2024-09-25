//玩家等待状态
using UnityEngine;

public class PlayerIdleState :PlayerGroundedState
{
    public PlayerIdleState(Player _player, PlayerStateMachine _stateMachine, string _animeBoolName) : base(_player, _stateMachine, _animeBoolName)
    {
        //已经实现了父类的构造函数
    }

    //进入
    public override void Enter()
    {
        base.Enter();
        //角色材质没有摩擦力，进入idle状态，把x轴的惯性重置为0
        player.ZeroVelocity();
    }

    //更新
    public override void Update()
    {
        base.Update();
    }
    
    //退出
    public override void Exit()
    {
        base.Exit();
    }
}
