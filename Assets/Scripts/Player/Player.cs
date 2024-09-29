//玩家控制

using System;
using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;

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
    //移动速度
    public float moveSpeed = 12f;
    //掉落速度
    public float jumpForce;
    
    [Header("冲刺相关")]
    //冲刺速度
    public float dashSpeed;
    //冲刺时间
    public float dashDuration;
    //冲刺方向
    public float dashDir;

    //玩家控制系统
    public PlayerInputControl inputControl;
    
    //技能管理
    public SkillManager skill { get; private set; }


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
        //玩家瞄准剑状态
        playerCatchSwordState = new PlayerCatchSwordState(this, playerStateMachine, "CatchSword");

        //创建一个实例
        inputControl = new PlayerInputControl();
    }

    protected override void Start()
    {
        base.Start();

        skill = SkillManager.instance;
        
        //初始化状态机--等待
        playerStateMachine.initialize(playerIdleState);
        //冲刺监听
        inputControl.Player.Dash.started += Dash;
    }
    
    protected override void Update()
    {
        base.Update();
        //执行更新状态机里面当前动画的更新
        playerStateMachine.currentState.Update();
    }
    
    //携程--暂停一段时间执行
    public IEnumerator BusyFor(float _seconds)
    {
        isBusy = true;
        
        yield return new WaitForSeconds(_seconds);
        
        isBusy = false;
    }
    
    private void OnEnable()
    {
        //启动玩家控制
        inputControl.Enable();
    }

    private void OnDisable()
    {
        //关闭玩家控制
        inputControl.Disable();
    }

    //调用玩家触发器--动画触发
    public void AnimationTrigger() => playerStateMachine.currentState.AnimationFinishTrigger();
    
    //玩家冲刺
    private void Dash(InputAction.CallbackContext obj)
    {
        // 检测到墙，不允许冲刺
        if (IsWallDetected())
        {
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
}