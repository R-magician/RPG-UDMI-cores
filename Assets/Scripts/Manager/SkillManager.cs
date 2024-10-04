//技能管理器
using System;
using UnityEngine;

public class SkillManager : MonoBehaviour
{
    public static SkillManager instance;

    //冲刺技能
    public DashSkill dash{get; private set;}
    //克隆技能
    public CloneSkill clone{get; private set;}
    //飞剑技能
    public SwordSkill sword { get; private set; }
    //黑洞技能
    public BlackholeSkill blackhole { get; private set; }

    private void Awake()
    {
        if (instance != null)
        {
            Destroy(instance.gameObject);
        }
        else
        {
            instance = this;
        }
        
        //获取挂载的组件
        dash = GetComponent<DashSkill>();
        clone = GetComponent<CloneSkill>();
        sword = GetComponent<SwordSkill>();
        blackhole = GetComponent<BlackholeSkill>();
    }
    
}
