//飞刀技能控制

using System;
using System.Collections.Generic;
using UnityEngine;

public class SwordSkillControler : MonoBehaviour
{
    //飞剑返回速度
    [SerializeField] private float returnSpeed =12f;
    
    //动画控制器
    private Animator anim;
    private Rigidbody2D rb;
    private CircleCollider2D cd;
    private Player player;
    
    //是否可以旋转
    private bool canRotate = true;
    //正在返回状态
    private bool isReturning;
    
    //弹跳速度
    public float bounceSpeed;
    //是否在弹跳
    public bool isBouncing = true;
    //反弹的次数
    public int amountOfBouce = 4;
    //目标敌人列表
    public List<Transform> enemyTarget;
    //目标索引
    private int targetIndex;

    private void Awake()
    {
        anim = GetComponentInChildren<Animator>();
        rb = GetComponent<Rigidbody2D>();
        cd = GetComponent<CircleCollider2D>();
    }

    //开始飞剑--方向，重力
    public void SetupSword(Vector2 dir,float gravityScale,Player _player)
    {
        player = _player;
        rb.linearVelocity = dir;
        rb.gravityScale = gravityScale;
        
        //播放剑旋转动画
        anim.SetBool("Rotation",true);
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
            transform.position = Vector2.MoveTowards(transform.position, player.transform.position+ new Vector3(.5f*player.facingDir,1f,0), Time.deltaTime * returnSpeed);

            //如果剑与玩家之间的距离小于1
            if (Vector2.Distance(transform.position, player.transform.position) < 2f)
            {
                //抓住剑
                player.CatchTheSword();
            }
        }
        
        //在弹跳
        if (isBouncing && enemyTarget.Count > 0)
        {
            //按时间返回一个点到另一个点的位置
            transform.position = Vector2.MoveTowards(transform.position,enemyTarget[targetIndex].position,bounceSpeed*Time.deltaTime);

            //两点之间的距离小于0.5
            if (Vector2.Distance(transform.position, enemyTarget[targetIndex].position) < 0.1f)
            {
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

        if (other.GetComponent<Enemy>() != null)
        {
            //检测到敌人，在弹跳，列表为空
            if (isBouncing && enemyTarget.Count <= 0)
            {
                //创建碰撞体--捡侧范围中有多少敌人，放入集合中
                Collider2D[] colliders = Physics2D.OverlapCircleAll(other.transform.position, 10f);

                foreach (var hit in colliders)
                {
                    if (hit.GetComponent<Enemy>() != null)
                    {
                        enemyTarget.Add(hit.transform);
                    }
                }
            }
        }

        StuckInfo(other);
    }

    //插入
    private void StuckInfo(Collider2D other)
    {
        //当前碰撞器接触到物体
        canRotate = false;
        //碰撞器关闭
        cd.enabled = false;
        
        //刚体类型改为运动型
        rb.bodyType  = RigidbodyType2D.Kinematic;
        //冻结所有轴
        rb.constraints = RigidbodyConstraints2D.FreezeAll;

        //正在弹跳,有攻击目标
        if (isBouncing && enemyTarget.Count > 0)
        {
            return;
        }
        //遇到碰撞物，关闭剑旋转动画
        anim.SetBool("Rotation",false);
        //将父级设置为碰撞物
        transform.parent = other.transform;
    }
}
