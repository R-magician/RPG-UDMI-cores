//冻结敌人并获得护甲效果
using UnityEngine;

[CreateAssetMenu(fileName = "冻结敌人",menuName = "数据/物品特效/冻结敌人")]
public class FreezeEnemies_Effect : ItemEffect
{
    //持续时间
    [SerializeField] private float duration;

    public override void ExecuteEffect(Transform _transform)
    {
        PlayerStats playerStats = PlayerManager.instance.Player.GetComponent<PlayerStats>();

        if (playerStats.currentHealth > playerStats.GetMaxHealthValue() * 0.1)
        {
            //当前血量大于10%，退出
            return;
        } 
        
        if (!Inventory.instance.CanUseArmor())
        {
            //在冷却中
            return;
        }
        
        //创建一个圆形检测，获取所在范围的碰撞-这将只存在一帧
        Collider2D[] colliders = Physics2D.OverlapCircleAll(_transform.position, 2);
        
        foreach (var hit in colliders)
        {
            //冻结敌人
            hit.GetComponent<Enemy>()?.FreezeTimeFor(duration);
        }
    }
}
