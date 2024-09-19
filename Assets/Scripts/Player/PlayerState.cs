//玩家动画状态
using UnityEngine;

public class PlayerState 
{
    //记录玩家状态
    protected PlayerStateMachine stateMachine;
    //玩家
    protected Player player;
    //为了方便赋值
    protected Rigidbody2D rb;
    
    //播放动画
    private string animBoolName;
    //冲刺时间
    protected float stateTime;
    
    //移动方向
    protected Vector2 inputDirection;

    //构造函数
    public PlayerState(Player _player, PlayerStateMachine _stateMachine, string _animeBoolName)
    {
        this.player = _player;
        this.stateMachine = _stateMachine;
        this.animBoolName = _animeBoolName;
    }

    //刚进入状态
    public virtual void Enter()
    {
        Debug.Log("开始 "+animBoolName+" 动画");
        player.anim.SetBool(animBoolName, true);
        
        rb = player.rb;
    }
    
    //动画进行更新
    public virtual void Update()
    {
        //持续更新冲刺时间
        stateTime -= Time.deltaTime;
        
        //获取移动时候的值
        inputDirection = player.inputControl.Player.Move.ReadValue<Vector2>();
        //通过上升下降的速度值传递给动画器--这样在其他状态中如果y值有变化也会更新值
        player.anim.SetFloat("yVelocity",rb.linearVelocity.y);
        Debug.Log("更新 "+animBoolName+" 动画");
    }
    
    //退出动画
    public virtual void Exit()
    {
        Debug.Log("动画 "+animBoolName+" 退出");
        player.anim.SetBool(animBoolName, false);
    }
}
