//残影技能控制

using System;
using UnityEngine;
using Random = UnityEngine.Random;

public class CloneSkillControler : MonoBehaviour
{
    //获取自身的精灵
    private SpriteRenderer sr;
    private Animator anim;
    //颜色透明度
    [SerializeField] private float colorLoosingSpeed;
    //克隆定时器
    private float cloneTimer;
    
    //攻击检查
    [SerializeField] private Transform attackCheck;
    //攻击范围
    [SerializeField] private float attackCheckRadius = .8f;
    //最近的敌人
    private Transform closestEnemy;

    private void Awake()
    {
        sr = GetComponent<SpriteRenderer>();
        anim = GetComponent<Animator>();
    }

    private void Update()
    {
        cloneTimer -= Time.deltaTime;
        if (cloneTimer < 0)
        {
            sr.color = new Color(1f, 1f, 1f, sr.color.a - (Time.deltaTime * colorLoosingSpeed));

            //当透明度看不见的时候彻底消失克隆体
            if (sr.color.a <= 0f)
            {
                Destroy(this.gameObject);
            }
        }
    }

    //开始克隆
    public void SetupClone(Transform newTransform,float _cloneDuration,bool _canAttack,Vector3 _offset,Transform _closestEnemy)
    {
        //是否可以攻击
        if (_canAttack)
        {
            //随机攻击动画
            anim.SetInteger("AttackNumber",Random.Range(1,4));
        }
        transform.position= newTransform.position + _offset;
        cloneTimer = _cloneDuration;

        closestEnemy = _closestEnemy;
        FaceCloseTarget();
    }
    
    //让克隆定时器停止
    private void AnimationTrigger()
    {
        cloneTimer = -.1f;
    }

    //复制体攻击触发器
    private void AttackTrigger()
    {
        //创建一个圆形检测，获取所在范围的碰撞-这将只存在一帧
        Collider2D[] colliders = Physics2D.OverlapCircleAll(attackCheck.position, attackCheckRadius);

        foreach (var hit in colliders)
        {
            //如果检测到敌人
            if (hit.GetComponent<Enemy>() != null)
            {
                //执行受伤
                hit.GetComponent<Enemy>().Damage();
            }
        }
    }

    //面对目标--创建碰撞区，让克隆体面向敌人
    private void FaceCloseTarget()
    {
        //最近的敌人null
        if (closestEnemy != null)
        {
            if (transform.position.x > closestEnemy.position.x)
            {
                transform.Rotate(0,180,0);
            }
        }
    }
}
