//冰火特效
using UnityEngine;
using UnityEngine.Serialization;

[CreateAssetMenu(fileName = "冰火效果",menuName = "数据/物品特效/冰火效果")]
public class IceAndFire_Effect : ItemEffect
{
    //效果预制体
    [SerializeField] private GameObject iceAndFirePrefab;
    [FormerlySerializedAs("velocity")] [SerializeField] private float xVelocity;

    public override void ExecuteEffect(Transform _respawnPosition)
    {
        Player player = PlayerManager.instance.Player;
        
        //攻击三次
        bool thirdAttack = player.playerPrimaryAttack.comboCounter == 2;

        //攻击三次
        if (thirdAttack)
        {
            GameObject newIceAndFirePrefab = Instantiate(iceAndFirePrefab, _respawnPosition.position, player.transform.rotation);
            //赋予速度
            newIceAndFirePrefab.GetComponent<Rigidbody2D>().linearVelocity = new Vector2(xVelocity * player.facingDir,0);
            
            Destroy(newIceAndFirePrefab,5);
        }
        
    }
}
