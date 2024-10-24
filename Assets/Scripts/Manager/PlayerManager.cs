//玩家管理器
using UnityEngine;

public class PlayerManager : MonoBehaviour
{
    //单例模式
    public static PlayerManager instance;

    //玩家
    public Player Player;

    private void Awake()
    {
        //只有一个实例，若有了销毁 
        if (instance != null)
        {
            Destroy(instance.gameObject);
        }
        else
        {
            instance = this;
        }
    }
}