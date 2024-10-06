//人物统计

using System;
using UnityEngine;

public class CharacterStats : MonoBehaviour
{
    //影响强度
    public Stat strength;
    //伤害
    public Stat damage;
    //闪避
    public Stat maxHealth;
    
    //当前血量
    [SerializeField]private int currentHealth;

    protected virtual void Awake()
    {
        currentHealth = maxHealth.GetValue();
    }

    //造成伤害--目标数据
    public virtual void DoDamage(CharacterStats _targetStats)
    {
        int totalDamage = strength.GetValue() + damage.GetValue();
        Debug.Log(totalDamage);
        _targetStats.TakeDamage(totalDamage);
    }

    //受到伤害
    public virtual void TakeDamage(int _damage)
    {
        currentHealth -= _damage;
        if (currentHealth <= 0)
        {
            Die();
        }
    }

    //执行死亡
    protected virtual void Die()
    {
        
    }
}
