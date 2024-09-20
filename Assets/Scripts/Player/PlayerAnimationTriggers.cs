//玩家动画触发器
using UnityEngine;

public class PlayerAnimationTriggers : MonoBehaviour
{
    //定义了一个只读变量--获取父组件上的Player主键
    private Player player => GetComponentInParent<Player>();
    
    private void AnimationTrigger()
    {
        //调用player上的触发器
        player.AnimationTrigger();
    }
}
