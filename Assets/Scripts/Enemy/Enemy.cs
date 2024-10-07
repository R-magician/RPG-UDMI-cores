//敌人基类
using System;
using System.Collections;
using UnityEngine;

public class Enemy : Enity
{
    //检测图层是否是player层
    [SerializeField] protected LayerMask whatIsPlayer;

    [Header("眩晕信息")] 
    //眩晕时长
    public float stunDuration;
    //眩晕方向
    public Vector2 stunDirection;
    //是否能被反击
    protected bool canBeStunned;
    //反击时的图片
    [SerializeField] protected GameObject counterImage;
    
    [Header("移动信息")]
    //移动速度
    public float moveSpeed;
    //等待时间
    public float idleTime;
    //战斗时间
    public float battleTime; 
    //默认速度
    private float defaultMoveSpeed;
    
    [Header("攻击信息")]
    //攻击距离
    public float attackDistance;
    //攻击冷却
    public float attackCooldown;
    //攻击最后时间
    [HideInInspector]public float lastTimeAttacked;

    //新建状态机
    public EnemyStateMachine stateMachine;
    
    //最后一个动画名称
    public string lastAnimBoolName{ get; private set; }

    protected override void Awake()
    {
        base.Awake();
        stateMachine = new EnemyStateMachine();
        defaultMoveSpeed = moveSpeed;
    }

    protected override void Update()
    {
        base.Update();
        //更新状态里面的更新
        stateMachine.currentState.Update();
    }

    public virtual void AssignlastAnimName(String _animName)
    {
        lastAnimBoolName = _animName;
    }
    
    //冻结时间
    public virtual void FreezeTime(bool timeFreeze)
    {
        if (timeFreeze)
        {
            //如果是冻结，移动速度为0
            moveSpeed = 0f;
            //动画的播放速度也暂停
            anim.speed = 0f;
        }
        else
        { 
            moveSpeed = defaultMoveSpeed;
            anim.speed = 1;
        }
    }

    //携程，冻结几秒
    protected virtual IEnumerator FreezeTimerFor(float seconds)
    {
        FreezeTime(true);
        yield return new WaitForSeconds(seconds);
        FreezeTime(false);
    }
    
    #region 反击
    
    //打开反击窗口
    public virtual void openCounterAttackWindow()
    {
        //在反击
        canBeStunned = true;
        //显示图片
        counterImage.SetActive(true);
    }

    //关闭反击窗口
    public virtual void CloseCounterAttackWindow()
    {
        canBeStunned = false;
        //关闭显示图片
        counterImage.SetActive(false);
    }
    #endregion

    //能否被反击
    public virtual bool CanBeStunned()
    {
        //能否被反击
        if (canBeStunned)
        {
            //关闭反击窗口
            CloseCounterAttackWindow();
            return true;
        }

        return false;
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
