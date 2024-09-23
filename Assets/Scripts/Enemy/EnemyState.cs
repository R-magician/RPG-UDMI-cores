//敌人状态
using UnityEngine;

public class EnemyState
{
    //创建状态机
    protected EnemyStateMachine stateMachine;
    //敌人
    protected Enemy enemyBase;
    protected Rigidbody2D rb;
    
    //当前状态动画
    private string animBoolName;
    
    //状态时间
    protected float stateTimer;
    //触发器状态
    protected bool triggerCalled;

    public EnemyState(Enemy _enemyBase, EnemyStateMachine _stateMachine, string _animBoolName)
    {
        stateMachine = _stateMachine;
        enemyBase = _enemyBase;
        animBoolName = _animBoolName;
    }

    //开始执行
    public virtual void Enter()
    {
        triggerCalled = false;
        rb = enemyBase.rb;
        enemyBase.anim.SetBool(animBoolName,true);
    }
    
    //更新执行
    public virtual void Update()
    {
        stateTimer -= Time.deltaTime;
    }
    
    //退出执行
    public virtual void Exit()
    {
        enemyBase.anim.SetBool(animBoolName,false);
    }

    //动画完成的时候触发-比如：(攻击玩，在动画时间轴中可以添加时间)
    public virtual void AnimationFinishTrigger()
    {
        triggerCalled = true;
    }
}
