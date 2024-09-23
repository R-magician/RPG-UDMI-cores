//玩家动画触发器
using UnityEngine;

public class SkeletonAnimationTriggers : MonoBehaviour
{
    //骷髅
    private Enemy_Skeleton enemy => GetComponentInParent<Enemy_Skeleton>();

    private void AnimationTrigger()
    {
        enemy.AnimationFinishTrigger();  
    }

}
