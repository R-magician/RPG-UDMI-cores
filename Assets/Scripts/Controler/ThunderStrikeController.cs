//闪电一击控制器

using System;
using UnityEngine;

public class ThunderStrikeController : MonoBehaviour
{
    protected virtual void OnTriggerEnter2D(Collider2D other)
    {
        if (other.GetComponent<Enemy>() != null)
        {
            //获取玩家数值
            PlayerStats playerStats = PlayerManager.instance.Player.GetComponent<PlayerStats>();
            
            EnemyStats enemyTarget = other.GetComponent<EnemyStats>();
            //检测到敌人
            playerStats.DoMagicalDamage(enemyTarget);
        }
    }
}
