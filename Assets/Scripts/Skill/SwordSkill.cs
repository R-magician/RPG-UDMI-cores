using UnityEngine;

public class SwordSkill : Skill
{
    [Header("技能信息")] 
    //剑预制体
    [SerializeField] private GameObject swordPrefab;
    //发射方向
    [SerializeField] private Vector2 launchDir;
    //飞行重力
    [SerializeField] private float swordGravity;

    //创建技能
    public void CreateSword()
    {
        
    }
}
