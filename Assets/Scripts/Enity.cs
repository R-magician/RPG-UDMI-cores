//实体类

using System.Collections;
using UnityEngine;

public class Enity : MonoBehaviour
{
   
   [Header("组件")]
   //动画器
   public Animator anim { get; private set; }
   public Rigidbody2D rb ;
   public EnityFX fx { get; private set; }

   [Header("击退信息")] 
   //击退方向
   [SerializeField] protected Vector2 knockbackDirection;
   //击退间隔时长
   [SerializeField] protected float knockbackDuration;
   //是否被击退
   protected bool isKnockback;
   
   [Header("检测信息")]
   //命中判定
   public Transform attackCheck;
   //攻击检测半径
   public float attackCheckRadius;
   //地面检测
   [SerializeField]
   protected Transform groundCheck;
   //地面检测距离
   [SerializeField] protected float groundCheckDistance;
   //墙体检测
   [SerializeField] protected Transform wallCheck;
   //墙体检测距离
   [SerializeField] protected float wallCheckDistance;
   //地面图层是哪一个
   [SerializeField] protected LayerMask whatIsGround;
   
   //角色方向
   public int facingDir { get; private set; } = 1;
   //角色是否翻转-是
   protected bool facingRight = true;
   
   //初始化执行
   protected virtual void Awake()
   {
      
   }
   
   //开始执行
   protected virtual void Start()
   {
      fx = GetComponent<EnityFX>();
      //获取子节点的Animator
      anim = GetComponentInChildren<Animator>();
      rb = GetComponent<Rigidbody2D>();
   }
   
   //更新执行
   protected virtual void Update()
   {
      
   }

   //受伤
   public virtual void Damage()
   {
      //调用fx组件中的携程--动画特效
      fx.StartCoroutine("FlashFx");
      StartCoroutine("HitKnockback");
   }

   //击退携程
   protected virtual IEnumerator HitKnockback()
   {
      isKnockback = true;
      rb.linearVelocity = new Vector2(knockbackDirection.x * -facingDir, knockbackDirection.y);
      yield return new WaitForSeconds(knockbackDuration);
      isKnockback = false;
   }
   
   #region 玩家速度

   //设置移动速度
   public void SetVelocity(float xVelocity, float yVelocity)
   {
      //被击退不能移动
      if (isKnockback)
      {
         return;
      }
      rb.linearVelocity = new Vector2(xVelocity, yVelocity);
        
      //移动的时候永远更新角色方向
      FlipController(xVelocity);
   }
    
   //将速度归至0
   public void ZeroVelocity() 
   {
      SetVelocity(0f, 0f);
   }

   //停止x轴上的速度
   public void SetZeroVelocityX()
   {
      SetVelocity(0, rb.linearVelocity.y);
   }


   #endregion
   
   #region 碰撞区域

   //是否检测到地面--这种不用在updata中没帧执行，在需要值的地方调用一下就行
   public virtual bool IsGroundDetected() =>
      Physics2D.Raycast(groundCheck.position, Vector2.down, groundCheckDistance, whatIsGround);
    
   //是否检测到墙面
   public virtual bool IsWallDetected() => Physics2D.Raycast(wallCheck.position, Vector2.right * facingDir, wallCheckDistance, whatIsGround);
    
   //绘制检测区域
   protected virtual void OnDrawGizmos()
   {
      //检测地面碰撞
      Gizmos.DrawLine(groundCheck.position,
         new Vector3(groundCheck.position.x, groundCheck.position.y - groundCheckDistance));
      //检测墙体碰撞
      Gizmos.DrawLine(wallCheck.position,
         new Vector3(wallCheck.position.x + wallCheckDistance, wallCheck.position.y));
      //攻击范围检测
      Gizmos.DrawWireSphere(attackCheck.position,attackCheckRadius);
   }

   #endregion
   
   #region 角色翻转

   //翻转角色
   public virtual void Flip()
   {
      facingDir = facingDir * -1;
      facingRight = !facingRight;
      //控制角色本身旋转180°
      transform.Rotate(0, 180, 0);
   }

   //翻转控制-单独提供一个参数是为了后面可能有什么特殊操作，比如（跳起来不能转方向）
   public virtual void FlipController(float x)
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

   #endregion
}
