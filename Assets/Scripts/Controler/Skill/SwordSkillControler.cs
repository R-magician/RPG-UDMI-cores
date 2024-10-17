//飞刀技能控制

using System;
using System.Collections.Generic;
using UnityEngine;

public class SwordSkillControler : MonoBehaviour
{
    //动画控制器
    private Animator anim;
    private Rigidbody2D rb;
    private CircleCollider2D cd;
    private Player player;

    //是否可以旋转
    private bool canRotate = true;

    //冻结时间
    private float freezeTimeDuration;
    //飞剑返回速度
    private float returnSpeed = 12f;
    
    //正在返回状态
    private bool isReturning;

    [Header("穿透信息")]
    //穿透数量
    private float pierceAmount;

    [Header("弹跳信息")]
    //弹跳速度
    private float bounceSpeed;
    //是否在弹跳
    private bool isBouncing;
    //反弹的次数
    private int amountOfBouce;
    //目标敌人列表
    private List<Transform> enemyTarget;
    //目标索引
    private int targetIndex;

    [Header("旋转信息")] 
    //最大移动距离
    private float maxTravelDistance;
    //旋转时间
    private float spinDuration;
    //旋转计算器
    private float spinTimer;
    //是否被停止
    private bool wasStopped;
    //是否旋转
    private bool isSpining;

    //伤害计时器
    private float hitTimer;
    //伤害冷却
    private float hitCooldown;
    
    //旋转方向
    private float spinDirection;

    private void Awake()
    {
        anim = GetComponentInChildren<Animator>();
        rb = GetComponent<Rigidbody2D>();
        cd = GetComponent<CircleCollider2D>();
    }

    //销毁飞剑
    private void DestroyMe()
    {
        Destroy(gameObject);
    }

    //设置飞剑--方向，重力
    public void SetupSword(Vector2 dir, float gravityScale, Player _player,float _freezeTimeDuration,float _returnSpeed)
    {
        player = _player;
        freezeTimeDuration = _freezeTimeDuration;
        returnSpeed = _returnSpeed;
        
        rb.linearVelocity = dir;
        rb.gravityScale = gravityScale;

        //当没有穿透数量的时候播放 飞剑旋转
        if (pierceAmount <= 0)
        {
            //播放剑旋转动画
            anim.SetBool("Rotation", true);
        }

        //匀速递减
        spinDirection = Mathf.Clamp(rb.linearVelocity.x, -1, 1);

        //7秒后执行
        Invoke("DestroyMe",7f);
    }

    //设置弹跳
    public void SetupBounce(bool _isBouncing, int _amountOfBouces,float _bounceSpeed)
    {
        isBouncing = _isBouncing;
        amountOfBouce = _amountOfBouces;
        bounceSpeed = _bounceSpeed;
        
        enemyTarget = new List<Transform>();
    }

    //设置穿透
    public void SetupPierce(int _pierceAmount)
    {
        pierceAmount = _pierceAmount;
    }
    
    //设置旋转
    public void SetupSpin(bool _isSpining, float _maxTravelDistance, float _spinDuration,float _hitCooldown)
    {
        isSpining = _isSpining;
        maxTravelDistance = _maxTravelDistance;
        spinDuration = _spinDuration;
        hitCooldown = _hitCooldown;
    }

    //剑返回到player
    public void ReturnSword()
    {
        //rb.bodyType  = RigidbodyType2D.Dynamic;
        //冻结所有旋转轴
        rb.constraints = RigidbodyConstraints2D.FreezeAll;
        transform.parent = null;

        //正在返回中
        isReturning = true;
    }
    private void Update()
    {

        if (canRotate)
        {
            transform.right = rb.linearVelocity;
        }

        //返回剑
        if (isReturning)
        {
            transform.position = Vector2.MoveTowards(transform.position,
                player.transform.position + new Vector3(.5f * player.facingDir, 1f, 0), Time.deltaTime * returnSpeed);

            //如果剑与玩家之间的距离小于1
            if (Vector2.Distance(transform.position, player.transform.position) < 2f)
            {
                //抓住剑
                player.CatchTheSword();
            }
        }
        //反弹的逻辑
        BounceLogic();

        //旋转滞空逻辑
        SpinLogic();
    }

    private void SpinLogic()
    {
        if (isSpining)
        {
            //在旋转
            if (Vector2.Distance(player.transform.position, transform.position) > maxTravelDistance && !wasStopped)
            {
                //停止旋转移动
                StopWhenSpinning();
            }

            if (wasStopped)
            {
                //旋转停止了
                spinTimer -= Time.deltaTime;
                
                //让飞剑向前飞点
                transform.position = Vector2.MoveTowards(transform.position, new Vector2(transform.position.x + spinDirection, transform.position.y), Time.deltaTime * 1.5f);
                
                if (spinTimer < 0)
                {
                    isReturning = true;
                    isSpining = false;
                }
                hitTimer -= Time.deltaTime;
                if (hitTimer < 0)
                {
                    hitTimer = hitCooldown;
                    //创建碰撞体--捡侧范围中有多少敌人，放入集合中
                    Collider2D[] colliders = Physics2D.OverlapCircleAll(transform.position, 1f);

                    foreach (var hit in colliders)
                    {
                        if (hit.GetComponent<Enemy>() != null)
                        {
                            SwordSkillDamage(hit.GetComponent<Enemy>());
                        }
                    }
                }
            }
        }
    }

    //停止旋转移动
    private void StopWhenSpinning()
    {
        //玩家和飞剑的位置大于最大旋转距离并且没有停止
        wasStopped = true;
        //冻结刚体位置
        rb.constraints = RigidbodyConstraints2D.FreezePosition;
        spinTimer = spinDuration;
    }

    //反弹的逻辑
    private void BounceLogic()
    {
        //在弹跳
        if (isBouncing && enemyTarget.Count > 0)
        {
            //按时间返回一个点到另一个点的位置
            transform.position = Vector2.MoveTowards(transform.position, enemyTarget[targetIndex].position,
                bounceSpeed * Time.deltaTime);

            //两点之间的距离小于0.5
            if (Vector2.Distance(transform.position, enemyTarget[targetIndex].position) < 0.1f)
            {
                SwordSkillDamage(enemyTarget[targetIndex].GetComponent<Enemy>());
                //切换到下一个
                targetIndex++;

                //反弹次数越来越少
                amountOfBouce--;

                if (amountOfBouce == 0)
                {
                    //弹跳结束
                    isBouncing = false;
                    //开始返回
                    isReturning = true;
                }

                if (targetIndex >= enemyTarget.Count)
                {
                    targetIndex = 0;
                }
            }
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        //剑在返回中不会触发后面的逻辑
        if (isReturning)
        {
            return;
        }

        if (other.GetComponent<Enemy>()!=null)
        {
            Enemy enemy = other.GetComponent<Enemy>();
            //执行受伤
            SwordSkillDamage(enemy);
        }

        SetupTargetsForBounce(other);

        StuckInfo(other);
    }

    //敌人受到飞剑伤害
    private void SwordSkillDamage(Enemy enemy)
    {
        player.stats.DoDamage(enemy.GetComponent<CharacterStats>());
        //执行携程
        enemy.FreezeTimeFor(freezeTimeDuration);
        
        //项链
        ItemDataEquipment equipedAmulet = Inventory.instance.GetEquipment(EquipmentType.Amulet);

        if (equipedAmulet != null)
        {
            equipedAmulet.Effect(enemy.transform);
        }
    }

    private void SetupTargetsForBounce(Collider2D other)
    {
        if (other.GetComponent<Enemy>() != null)
        {
            //检测到敌人，在弹跳，列表为空
            if (isBouncing && enemyTarget.Count <= 0)
            {
                //创建碰撞体--捡侧范围中有多少敌人，放入集合中
                Collider2D[] colliders = Physics2D.OverlapCircleAll(transform.position, 10f);

                foreach (var hit in colliders)
                {
                    if (hit.GetComponent<Enemy>() != null)
                    {
                        enemyTarget.Add(hit.transform);
                    }
                }
            }
        }
    }

    //插入墙体、敌人
    private void StuckInfo(Collider2D other)
    {
        //当有穿透数，并且是敌人
        if (pierceAmount>0 && other.GetComponent<Enemy>()!=null)
        {
            pierceAmount--;
            return;
        }

        //在旋转
        if (isSpining)
        {
            StopWhenSpinning();
            return;
        }
        
        //当前碰撞器接触到物体
        canRotate = false;
        //碰撞器关闭
        cd.enabled = false;

        //刚体类型改为运动型
        rb.bodyType = RigidbodyType2D.Kinematic;
        //冻结所有轴
        rb.constraints = RigidbodyConstraints2D.FreezeAll;

        //正在弹跳,有攻击目标
        if (isBouncing && enemyTarget.Count > 0)
        {
            return;
        }

        //遇到碰撞物，关闭剑旋转动画
        anim.SetBool("Rotation", false);
        //将父级设置为碰撞物
        transform.parent = other.transform;
    }
}