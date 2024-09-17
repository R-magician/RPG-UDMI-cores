//玩家控制

using System;
using UnityEngine;
using UnityEngine.InputSystem;

public class Player : MonoBehaviour
{
    [Header("移动相关")]
    public float moveSpeed = 12f;
    
    [Header("组件")]
    //动画器
    public Animator anim { get;private set; }

    public Rigidbody2D rb;
    //玩家控制系统
    public PlayerInputControl inputControl;
    
    
    [Header("玩家状态")]
    //声明一个状态机
    public PlayerStateMachine playerStateMachine { get; private set; }
    //等待状态
    public PlayerIdleState playerIdleState { get; private set; }
    //移动状态
    public PlayerMoveState playerMoveState { get; private set; }

    /// <summary>
    /// 初始化执行
    /// </summary>
    private void Awake()
    {
        //新建状态机
        playerStateMachine = new PlayerStateMachine();
        //新建等待状态--对应动画器中的变量
        playerIdleState = new PlayerIdleState(this, playerStateMachine,"Idle");
        //新建移动状态--对应动画器中的变量
        playerMoveState = new PlayerMoveState(this, playerStateMachine,"Move");
        
        //创建一个实例
        inputControl = new PlayerInputControl();
    }

    private void Start()
    {
        //获取子节点的Animator
        anim = GetComponentInChildren<Animator>();
        rb = GetComponent<Rigidbody2D>();
        //初始化状态机--等待
        playerStateMachine.initialize(playerIdleState);
    }

    private void Update()
    {
        //执行更新状态机里面当前动画的更新
        playerStateMachine.currentState.Update();
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
    
    //设置移动速度
    public void SetVelocity(float xVelocity, float yVelocity)
    {
     rb.linearVelocity = new Vector2(xVelocity, yVelocity);   
    }
}
