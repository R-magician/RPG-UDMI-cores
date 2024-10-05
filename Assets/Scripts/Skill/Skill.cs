using System;
using UnityEngine;
using UnityEngine.InputSystem;

public class Skill : MonoBehaviour
{
    //冷却时间
    [SerializeReference] protected float cooldown;
    //更新冷却时间
    protected float cooldownTimer;

    protected Player player;

    protected virtual void Awake()
    {
        player = PlayerManager.instance.Player;
    }

    protected virtual void Update()
    {
        //更新冷却
        cooldownTimer -= Time.deltaTime;
    }

    //是否能使用技能
    public virtual bool CanUseSkill()
    {
        if (cooldownTimer <= 0f)
        {
            //使用技能
            UseSkill();
            cooldownTimer = cooldown;
            return true;
        }
        
        //技能在冷却中
        return false;
    }

    //使用技能
    public virtual void UseSkill()
    {
        //做一些特定技能的事情
    }

    //找到最近的敌人
    protected virtual Transform FindClosestEnemy(Transform _checkTransform)
    {
        Collider2D[] colliders = Physics2D.OverlapCircleAll(_checkTransform.position, 25f);
        
        //默认等于无限大
        float closestDistance = Mathf.Infinity;
        
        Transform closestEnemy = null;

        foreach (var hit in colliders)
        {
            if (hit.GetComponent<Enemy>() != null)
            {
                //区域范围中用敌人--获取之间相隔的距离
                float distanceToEnemy = Vector2.Distance(_checkTransform.position, hit.transform.position);
                if (distanceToEnemy < closestDistance)
                {
                    //找到最近的那个敌人
                    closestDistance = distanceToEnemy;
                    closestEnemy = hit.transform;
                }
            }
        }

        return closestEnemy;
    }
}
