//闪电控制器

using System;
using Unity.Burst.Intrinsics;
using UnityEngine;

public class ThunderStrikeController : MonoBehaviour
{
    //目标数值统计
    [SerializeField] private CharacterStats targetStats;
    //传播速度
    [SerializeField] private float speed;
    //伤害值
    private int damage;
    
    private Animator anim;
    //是否被触发
    private bool triggered;

    private void Awake()
    {
        anim = GetComponentInChildren<Animator>();
    }

    //设置预制体参数
    public void Setup(int _damage, CharacterStats _targetStats)
    {
        damage = _damage;
        targetStats = _targetStats;
    }

    private void Update()
    {
        if (!targetStats)
        {
            return;
        }
        
        if (triggered)
        {
            return;
        }
        
        transform.position =
            Vector2.MoveTowards(transform.position, targetStats.transform.position, speed * Time.deltaTime);

        //目标和我们位置之间的方向
        transform.right = transform.position - targetStats.transform.position;
        
        //距离小于0.1
        if (Vector2.Distance(transform.position, targetStats.transform.position) < .1f)
        {
            //将旋转设置为0
            anim.transform.localRotation = Quaternion.identity;
            //播放的时候动画向上移.5个距离
            anim.transform.localPosition = new Vector3(0, .5f);
            
            //父对象的旋转也设置为0
            transform.localRotation = Quaternion.identity;
            transform.localScale = new Vector3(3, 3);
            
            //延迟执行
            Invoke("DamageAndSelfDestroy",.2f);
            
            triggered = true;
            anim.SetTrigger("Hit");
        }
    }

    //伤害对象，销毁本体
    private void DamageAndSelfDestroy()
    {
        //传递的电弧到目标身上，设置状态被电
        targetStats.ApplyShock(true);
        //受到伤害
        targetStats.TakeDamage(damage);
        //延迟销毁
        Destroy(gameObject,.4f);
    }
}
