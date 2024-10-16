//雷霆一击-特效
using UnityEngine;

[CreateAssetMenu(fileName = "打击效果",menuName = "数据/物品特效/雷霆一击")]
public class ThunderStrike_Effect : ItemEffect
{
    //特效预制体
    [SerializeField] private GameObject thunderStrikePrefab;
    public override void ExecuteEffect(Transform _enemyPosition)
    {
        //新增对象
        GameObject newThunderStrike = Instantiate(thunderStrikePrefab,_enemyPosition.position,Quaternion.identity);
        
        //销毁对象
        Destroy(newThunderStrike,1f);
    }
}
