using System.Collections;
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

    //在DashStart上创建Clone
    [SerializeField] private bool createCloneOnDashStart;
    //在Dash Over创建Clone
    [SerializeField] private bool createCloneOnDashOver;
    //在反击时创建克隆
    [SerializeField] private bool createCloneOnCounterAttack;

    //创建克隆体--传递位置
    public void CreateClone(Transform clonePosition,Vector3 offset)
    {
        GameObject newClone = Instantiate(clonePrefab);
        //在指定位置生成
        newClone.GetComponent<CloneSkillControler>().SetupClone(clonePosition,cloneDuration,canAttack,offset,FindClosestEnemy(newClone.transform));
    }

    //在Dash开始时创建克隆
    public void CreateCloneOnDashStart()
    {
        if (createCloneOnDashStart)
        {
            CreateClone(player.transform,Vector3.zero);
        }
    }

    //在Dash结束时创建克隆
    public void CreateCloneOnDashOver()
    {
        if (createCloneOnDashOver)
        {
            CreateClone(player.transform,Vector3.zero);
        }
    }
    
    //在反击时创建克隆
    public void CreateCloneOnCounterAttack(Transform _enemyTransform)
    {
        if (createCloneOnCounterAttack)
        {
            StartCoroutine(CreateCloneDelay(_enemyTransform, new Vector3(2 * player.facingDir, -1.1f)));
        }
    }

    //携程-创建克隆延迟
    private IEnumerator CreateCloneDelay(Transform _transform,Vector3 offset)
    {
        yield return new WaitForSeconds(.4f);
        CreateClone(_transform,offset);
    }
}
