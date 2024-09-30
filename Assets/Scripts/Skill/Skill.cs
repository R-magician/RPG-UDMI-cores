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
}
