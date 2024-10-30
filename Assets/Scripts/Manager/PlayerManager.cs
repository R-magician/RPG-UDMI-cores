//玩家管理器
using UnityEngine;

public class PlayerManager : MonoBehaviour
{
    //单例模式
    public static PlayerManager instance;

    //玩家
    public Player Player;

    //当前货币
    public int current;

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

    //是否有足够的货币
    public bool HaveEnoughMoney(int _price)
    {
        if (_price > current)
        {
            //没有足够的货币
            return false;
        }
        current -= _price;
        return true;
    }
}