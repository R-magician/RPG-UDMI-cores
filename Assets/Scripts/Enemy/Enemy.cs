//敌人基类
using System;
using UnityEngine;

public class Enemy : Enity
{
    //检测图层是否是player层
    [SerializeField] protected LayerMask whatIsPlayer;
    
    [Header("移动信息")]
    //移动速度
    public float moveSpeed;
    //等待时间
    public float idleTime;
    //战斗时间
    public float battleTime; 
    
    [Header("攻击信息")]
    //攻击距离
    public float attackDistance;
    //攻击冷却
    public float attackCooldown;
    //攻击最后时间
    [HideInInspector]public float lastTimeAttacked;

    //新建状态机
    public EnemyStateMachine stateMachine;

    protected override void Awake()
    {
        base.Awake();
        stateMachine = new EnemyStateMachine();
    }

    protected override void Update()
    {
        base.Update();
        //更新状态里面的更新
        stateMachine.currentState.Update();
    }
    
    public virtual void AnimationFinishTrigger()=>stateMachine.currentState.AnimationFinishTrigger();
    
    //是否检测到玩家
    public virtual RaycastHit2D isPlayerDetected() =>
        Physics2D.Raycast(wallCheck.position, Vector2.right * facingDir, 50, whatIsPlayer);

    protected override void OnDrawGizmos()
    {
        base.OnDrawGizmos();
        Gizmos.color = Color.yellow;
        Gizmos.DrawLine(transform.position,new Vector3(transform.position.x + attackDistance * facingDir,transform.position.y));
    }
}
