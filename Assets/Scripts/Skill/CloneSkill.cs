using UnityEngine;

public class CloneSkill : Skill
{
    
    [Header("克隆信息")]
    //克隆的预制体
    [SerializeField] private GameObject clonePrefab;
    //克隆持续时间
    [SerializeField] private float cloneDuration;
    //是否可以攻击
    [SerializeField] private bool canAttack;


    //创建克隆体--传递位置
    public void CreateClone(Transform clonePosition)
    {
        GameObject newClone = Instantiate(clonePrefab);
        //在指定位置生成
        newClone.GetComponent<CloneSkillControler>().SetupClone(clonePosition,cloneDuration,canAttack);
    }
}
