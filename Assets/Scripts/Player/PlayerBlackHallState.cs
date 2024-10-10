//玩家释放黑洞状态
using UnityEngine;

public class PlayerBlackHallState :PlayerState
{
    //飞行时间
    private float flyTime =.4f;
    //技能使用
    private bool skillUsed;
    //默认重力
    private float defaultGravity;
    
    public PlayerBlackHallState(Player _player, PlayerStateMachine _stateMachine, string _animeBoolName) : base(_player, _stateMachine, _animeBoolName)
    {
    }

    public override void Enter()
    {
        base.Enter();
        defaultGravity = rb.gravityScale;
        
        skillUsed = false;
        stateTimer = flyTime;
        rb.gravityScale = 0;
    }

    public override void Update()
    {
        base.Update();
        if (stateTimer > 0)
        {
            //使用技能是上升15个单位
            rb.linearVelocity = new Vector2(0, 15f);
        }

        if (stateTimer <0)
        {
            rb.linearVelocity = new Vector2(0, -.1f);

            //如果没有使用技能
            if (!skillUsed)
            {
                //释放黑洞---如果能使用技能
                if (player.skill.blackhole.CanUseSkill())
                {
                    skillUsed = true;
                }
            }
        }
        //如果已经完成黑洞技能，状态切换为air
        if (player.skill.blackhole.BlackholeCompleted())
        {
            stateMachine.ChangeState(player.playerAirState);
        }
    }

    public override void Exit()
    {
        base.Exit();
        player.rb.gravityScale = defaultGravity;
        //玩家设置透明
        player.fx.MakeTransparent(false);
    }
}
