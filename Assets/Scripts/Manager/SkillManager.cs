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
    }

    private void Start()
    {
        dash = GetComponent<DashSkill>();
        clone = GetComponent<CloneSkill>();
    }
}
