//玩家主要攻击
using UnityEngine;

public class PlayerPrimaryAttackState : PlayerState
{
    //组合攻击计数
    public int comboCounter { get; private set; }
    
    //最后的攻击时间
    private float lastTimeAttacked;
    
    //开始前需要多少时间来重置
    private float comboWindow = 2;
    
    public PlayerPrimaryAttackState(Player _player, PlayerStateMachine _stateMachine, string _animeBoolName) : base(_player, _stateMachine, _animeBoolName)
    {
    }


    public override void Enter()
    {
        base.Enter();
        
        //AudioManager.instance.PlaySFX(0);
        
        // 当组合攻击大于2或者当前时间>=最后攻击时间+间隔攻击时间
        if (comboCounter > 2 || Time.time >= lastTimeAttacked + comboWindow)
        {
            comboCounter = 0;
        }
        //切换连击动画
        player.anim.SetInteger("ComboCounter",comboCounter);
        //将动画速度提高
        player.anim.speed = 1.2f;
        
        //选择攻击方向
        float attackDir = player.facingDir;

        if (inputDirection.x != 0)
        {
            attackDir = inputDirection.x;
        }
        
        player.SetVelocity(player.attackMovement[comboCounter].x * attackDir,player.attackMovement[comboCounter].y);

        //状态时间初始值
        stateTimer = .1f;
    }

    public override void Update()
    {
        base.Update();
        //如果触发器被调用-改变玩家状态
        if (triggerCalled)
        {
            stateMachine.ChangeState(player.playerIdleState);
        }

        if (stateTimer < 0)
        {
            player.ZeroVelocity();
        }
        
    }

    public override void Exit()
    {
        base.Exit();
        
        //开始携程
        player.StartCoroutine("BusyFor",.15f);
        //恢复动画速度
        player.anim.speed = 1;
        
        comboCounter++;
        lastTimeAttacked = Time.time;
    }
}
