using UnityEngine;

public class Skill : MonoBehaviour
{
    //冷却时间
    [SerializeReference] protected float cooldown;
    //更新冷却时间
    protected float cooldownTimer;

    protected virtual void Update()
    {
        //更新冷却
        cooldownTimer -= Time.deltaTime;
    }
}
