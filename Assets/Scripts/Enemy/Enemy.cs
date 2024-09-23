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
    
    //是否检测到玩家
    public virtual RaycastHit2D isPlayerDetected() =>
        Physics2D.Raycast(wallCheck.position, Vector2.right * facingDir, 50, whatIsPlayer);
}
