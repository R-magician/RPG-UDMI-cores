//水晶技能控制

using System;
using UnityEngine;
using Random = UnityEngine.Random;

public class CrystalSkillController : MonoBehaviour
{
    private Animator anim => GetComponent<Animator>();
    private CircleCollider2D cd => GetComponent<CircleCollider2D>();
    
    //水晶持续时间
    private float crystalExistTimer;
    //会爆炸
    private bool canExplode;
    //会移动
    private bool canMove;
    //移动速度
    private float moveSpeed;

    //能否增长
    private bool canGrow;
    //增长速度
    private float growSpeed = 5f;
    
    //最近敌人的距离
    private Transform closestTarget;
    //敌人所在图层
    [SerializeField] private LayerMask whatIsEnemy;
    
    //设置水晶技能
    public void SetupCrystalSkill(float _crystalDuration, bool _canExplode, bool _canMoveToEnemy, float _moveSpeed,float _growSpeed,Transform _closestTarget)
    {
        crystalExistTimer = _crystalDuration;   
        canExplode = _canExplode;
        canMove = _canMoveToEnemy;
        moveSpeed = _moveSpeed;
        growSpeed = _growSpeed;
        closestTarget = _closestTarget;
    }

    //在范围内随机找到敌人
    public void ChooseRandomEnemy()
    {
        float radius = SkillManager.instance.blackhole.GetBlackholeRadius();
        //创建一个圆形检测，获取所在范围的碰撞-这将只存在一帧
        Collider2D[] colliders = Physics2D.OverlapCircleAll(transform.position, radius,whatIsEnemy);

        if (colliders.Length > 0)
        {
            closestTarget = colliders[Random.Range(0, colliders.Length)].transform;
        }
    }

    private void Update()
    {
        crystalExistTimer -= Time.deltaTime;
        if (crystalExistTimer < 0)
        {
            //能爆炸
            FinishCrystal();
        }

        //可以移动
        if (canMove)
        {
            //水晶向目标移动
            transform.position = Vector2.MoveTowards(transform.position, closestTarget.position, moveSpeed * Time.deltaTime);
            //水晶与敌人之间的距离小于1
            if (Vector2.Distance(transform.position, closestTarget.position) < 1f)
            {
                FinishCrystal();
                canMove = false;
            }
        }

        if (canGrow)
        {
            transform.localScale = Vector2.Lerp(transform.localScale, new Vector2(3,3), growSpeed * Time.deltaTime);
        }
    }

    //水晶状态
    public void FinishCrystal()
    {
        if(canExplode)
        {
            canGrow = true;
            anim.SetTrigger("Explode");
        }
        else
        {
            SelfDestroy();
        }
    }

    //产生伤害
    private void AnimationExplodeEvent()
    {
        //创建一个圆形检测，获取所在范围的碰撞-这将只存在一帧
        Collider2D[] colliders = Physics2D.OverlapCircleAll(transform.position, cd.radius);

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
    
    public void SelfDestroy()
    {
        Destroy(gameObject);
    }
}
