//玩家控制

using System;
using UnityEngine;
using UnityEngine.InputSystem;

public class Player : MonoBehaviour
{
    [Header("移动相关")]
    //移动速度
    public float moveSpeed = 12f;

    //掉落速度
    public float jumpForce;

    [Header("碰撞检测")]
    //地面检测
    [SerializeField]
    private Transform groundCheck;

    //地面检测距离
    [SerializeField] private float groundCheckDistance;

    //墙体检测
    [SerializeField] private Transform wallCheck;

    //墙体检测距离
    [SerializeField] private float wallCheckDistance;

    //地面图层是哪一个
    [SerializeField] private LayerMask whatIsGround;

    //角色方向
    public int facingDir { get; private set; } = 1;

    //角色是否翻转-是
    private bool facingRight = true;

    [Header("组件")]
    //动画器
    public Animator anim { get; private set; }

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

    //跳跃状态
    public PlayerJumpState playerJumpState { get; private set; }

    //空中状态
    public PlayerAirState playerAirState { get; private set; }

    //计算器
    public float timer;
    //计算器冷却时间
    public float cooldown;

    /// <summary>
    /// 初始化执行
    /// </summary>
    private void Awake()
    {
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
        
        timer -= Time.deltaTime;

        //冷却时间结束并且按下
        if (timer <= 0)
        {
            timer = cooldown;
        }
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
        
        //移动的时候永远更新角色方向
        FlipController(xVelocity);
    }

    //是否检测到地面--这种不用在updata中没帧执行，在需要值的地方调用一下就行
    public bool IsGroundDetected() =>
        Physics2D.Raycast(groundCheck.position, Vector2.down, groundCheckDistance, whatIsGround);

    //翻转角色
    public void Flip()
    {
        facingDir = facingDir * -1;
        facingRight = !facingRight;
        //控制角色本身旋转180°
        transform.Rotate(0, 180, 0);
    }

    //翻转控制-单独提供一个参数是为了后面可能有什么特殊操作，比如（跳起来不能转方向）
    public void FlipController(float x)
    {
        if (x > 0 && !facingRight)
        {
            //如果x轴的速度大于0，并且翻转了，翻转角色面向x
            Flip();
        }
        else if (x < 0 && facingRight)
        {
            //如果x轴的速度小于0，并且没有翻转，翻转角色面向-x
            Flip();
        }
    }

    //绘制检测区域
    private void OnDrawGizmos()
    {
        //检测地面碰撞
        Gizmos.DrawLine(groundCheck.position,
            new Vector3(groundCheck.position.x, groundCheck.position.y - groundCheckDistance));
        //检测墙体碰撞
        Gizmos.DrawLine(wallCheck.position,
            new Vector3(wallCheck.position.x + wallCheckDistance, wallCheck.position.y));
    }
}