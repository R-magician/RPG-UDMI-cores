//玩家控制

using System;
using System.Collections;
using UnityEngine;

public class Player : Enity
{
    [Header("攻击详情")]
    //攻击动作
    public Vector2[] attackMovement;
    //反击时间
    public float counterAttackDuration =.2f;
    
    //是否忙碌--经过携程该值
    public bool isBusy{get; private set;}
    
    [Header("移动相关")]
    //移动方向
    public Vector2 inputDirection;
    //移动速度
    public float moveSpeed = 12f;
    //掉落速度
    public float jumpForce;
    //回收剑冲击力
    public float swordReturnImpact;
    //默认速度
    private float defaultMoveSpeed;
    //默认跳跃力
    private float defaultJumpForce;
    
    [Header("冲刺相关")]
    //冲刺速度
    public float dashSpeed;
    //冲刺时间
    public float dashDuration;
    //默认冲刺速度
    private float defaultDashSpeed;
    //冲刺方向
    public float dashDir;
    
    //技能管理
    public SkillManager skill { get; private set; }
    //飞剑，只能保证有一个
    public GameObject sword{ get; private set; }


    [Header("玩家状态")]
    //声明一个状态机
    public PlayerStateMachine playerStateMachine { get; private set; }
    //等待状态
    public PlayerIdleState playerIdleState { get; private set; }
    //移动状态
    public PlayerMoveState playerMoveState { get; private set; }
    //跳跃状态
    public PlayerJumpState playerJumpState { get; private set; }
    //空中状态
    public PlayerAirState playerAirState { get; private set; }
    //滑墙状态
    public PlayerWallSlideState playerWallSlideState { get; private set; }
    //墙上跳跃状态
    public PlayerWallJumpState playerWallJumpState { get; private set; }
    //冲刺状态
    public PlayerDashState playerDashState { get; private set; }
    //玩家主要攻击
    public PlayerPrimaryAttackState playerPrimaryAttack { get; private set; }
    //玩家反击状态
    public PlayerCounterAttackState playerCounterAttackState { get; private set; }
    //玩家瞄准剑状态
    public PlayerAimSwordState playerAimSwordState { get; private set; }
    //玩家掌握剑的状态
    public PlayerCatchSwordState playerCatchSwordState { get; private set; }
    //玩家释放黑洞状态
    public PlayerBlackHallState playerBlackHallState { get; private set; }  
    //玩家死亡状态
    public PlayerDeadState playerDeadState { get; private set; }

    /// <summary>
    /// 初始化执行
    /// </summary>
    protected override void Awake()
    {
        base.Awake();
        //新建状态机
        playerStateMachine = new PlayerStateMachine();
        //新建等待状态--对应动画器中的变量
        playerIdleState = new PlayerIdleState(this, playerStateMachine, "Idle");
        //新建移动状态--对应动画器中的变量
        playerMoveState = new PlayerMoveState(this, playerStateMachine, "Move");
        //新建跳跃状态--对应动画器中的变量
        playerJumpState = new PlayerJumpState(this, playerStateMachine, "Jump");
        //新建停留空中状态--对应动画器中的变量
        playerAirState = new PlayerAirState(this, playerStateMachine, "Jump");
        //新建冲刺状态--对应动画器中的变量
        playerDashState = new PlayerDashState(this, playerStateMachine, "Dash");
        //新建滑墙状态--对应动画器中的变量
        playerWallSlideState = new PlayerWallSlideState(this, playerStateMachine, "WallSlide");
        //新建墙上跳跃状态--对应动画器中的变量
        playerWallJumpState = new PlayerWallJumpState(this, playerStateMachine, "Jump");

        //玩家主要攻击状态
        playerPrimaryAttack = new PlayerPrimaryAttackState(this, playerStateMachine, "Attack");
        //玩家反击状态
        playerCounterAttackState = new PlayerCounterAttackState(this, playerStateMachine, "CounterAttack");
        
        //玩家瞄准剑状态
        playerAimSwordState = new PlayerAimSwordState(this, playerStateMachine, "AimSword");
        //玩家抓住剑状态
        playerCatchSwordState = new PlayerCatchSwordState(this, playerStateMachine, "CatchSword");
        
        //玩家释放黑洞状态
        playerBlackHallState = new PlayerBlackHallState(this, playerStateMachine, "Jump");
        
        //玩家死亡状态
        playerDeadState = new PlayerDeadState(this, playerStateMachine, "Die");
    }

    protected override void Start()
    {
        base.Start();
        
        skill = SkillManager.instance;
        
        //初始化状态机--等待
        playerStateMachine.initialize(playerIdleState);

        defaultMoveSpeed = moveSpeed;
        defaultJumpForce = jumpForce;
        defaultDashSpeed = dashSpeed;
    }

    protected override void Update()
    {
        base.Update();
        //获取移动时候的值
        inputDirection = InputManager.instance.inputDirection;
        //执行更新状态机里面当前动画的更新
        playerStateMachine.currentState.Update();
    }

    public override void SlowEntityBy(float _slowPercentage, float _slowDuration)
    {
        moveSpeed = moveSpeed * (1 - _slowPercentage);
        jumpForce = jumpForce * (1 - _slowPercentage);
        dashSpeed = dashSpeed * (1 - _slowPercentage);
        anim.speed = anim.speed * (1 - _slowPercentage);
        //延迟执行
        Invoke("ReturnDefaultSpeed",_slowDuration);
    }

    protected override void ReturnDefaultSpeed()
    {
        base.ReturnDefaultSpeed();
        moveSpeed = defaultMoveSpeed;
        jumpForce = defaultJumpForce;
        dashSpeed = defaultDashSpeed;
    }

    //分配飞剑
    public void AssignNewSword(GameObject newSword)
    {
        sword = newSword;
    }
    
    //抓住飞剑
    public void CatchTheSword()
    {
        playerStateMachine.ChangeState(playerCatchSwordState);
        Destroy(sword);
    }
    
    //携程--暂停一段时间执行
    public IEnumerator BusyFor(float _seconds)
    {
        isBusy = true;
        
        yield return new WaitForSeconds(_seconds);
        
        isBusy = false;
    }

    //调用玩家触发器--动画触发
    public void AnimationTrigger() => playerStateMachine.currentState.AnimationFinishTrigger();
    
    //玩家冲刺
    public void Dash()
    {
        // 检测到墙，不允许冲刺
        if (IsWallDetected())
        {
            return;
        }

        if (!skill.dash.dashUnlocked)
        {
            //未学会技能
            return;
        }
        
        //在冷却时间中可以激活
        if (SkillManager.instance.dash.CanUseSkill())
        {
            //有冲刺方向
            if (dashDir == 0)
            {
                dashDir = facingDir;
            }
            playerStateMachine.ChangeState(playerDashState);
        }
        
    }
    
    //水晶
    public void Crystal()
    {
        //技能被解说才能使用
        if (skill.crystal.crystalUnlocked)
        {
            skill.crystal.CanUseSkill();
        }
    }

    //玩家死亡
    public override void Die()
    {
        base.Die();
        playerStateMachine.ChangeState(playerDeadState);
    }

    protected override void SetupZeroKnockbackPower()
    {
        base.SetupZeroKnockbackPower();
        knockbackPower = new Vector2(0, 0);
    }
}