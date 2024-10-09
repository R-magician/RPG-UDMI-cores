//闪电控制器

using System;
using UnityEngine;

public class ThunderStrikeController : MonoBehaviour
{
    //目标数值统计
    [SerializeField] private CharacterStats targetStats;
    //传播速度
    [SerializeField] private float speed;
    private Animator anim;
    //是否被触发
    private bool triggered;

    private void Awake()
    {
        anim = GetComponentInChildren<Animator>();
    }

    private void Update()
    {
        if (triggered)
        {
            return;
        }
        
        transform.position =
            Vector2.MoveTowards(transform.position, targetStats.transform.position, speed * Time.deltaTime);

        //目标和我们位置之间的方向
        transform.right = transform.position - targetStats.transform.position;
        
        if (Vector2.Distance(transform.position, targetStats.transform.position) < .1f)
        {
            triggered = true;
            //距离小于0.1--受到伤害
            targetStats.TakeDamage(1);
            anim.SetTrigger("Hit");
            //延迟销毁
            Destroy(gameObject,.4f);
        }
    }
}
