//疗伤效果
using UnityEngine;

[CreateAssetMenu(fileName = "治疗效果",menuName = "数据/物品特效/治疗效果")]
public class Heal_Effect : ItemEffect
{
    [Range(0f,1f)]
    //治疗百分比
    [SerializeField] private float healPercent;
    
    public override void ExecuteEffect(Transform _respawnPosition)
    {
        //获取玩家状态
        PlayerStats playerStats = PlayerManager.instance.Player.GetComponent<PlayerStats>();
        //决定要治疗多少
        int healAmount = Mathf.RoundToInt(playerStats.GetMaxHealthValue() * healPercent); 
        //实际治疗多少
        playerStats.IncreaseHealthBy(healAmount);
    }
}
