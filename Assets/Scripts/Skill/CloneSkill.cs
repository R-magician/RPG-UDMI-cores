using System;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class CloneSkill : Skill
{
    [Header("克隆信息")]
    // 攻击力系数
    [SerializeField] private float attackMultipler;
    //克隆的预制体
    [SerializeField] private GameObject clonePrefab;
    //克隆持续时间
    [SerializeField] private float cloneDuration;

    [Header("克隆攻击")] 
    [SerializeField] private UI_SkillTreeSlot cloneAttackButton;
    //克隆攻击系数--本体攻击力的几层
    [SerializeField] private float cloneAttackMultiplier;
    //是否可以攻击
    [SerializeField] private bool canAttack;
    
    [Header("侵略性克隆")]
    [SerializeField] private UI_SkillTreeSlot aggresiveAttackUnlockButton;
    //侵略性克隆攻击系数
    [SerializeField] private float aggresiveCloneAttackMultiplier;
    //可以应用命中效果
    public bool canApplyOnHitEffect { get; private set; }
    
    [Header("多重克隆")]
    [SerializeField] private UI_SkillTreeSlot multipleUnlockButton;
    //多重克隆攻击系数
    [SerializeField] private float multipleCloneAttackMultiplier;
    //可以复制克隆
    [SerializeField] private bool canDuplicateClone;
    //克隆概率
    private float chanceToDuplicate;
    
    [Header("水晶克隆")]
    [SerializeField] private UI_SkillTreeSlot crystalUnlockButton;
    //水晶代替克隆
    public bool crystalInseadOfClone;

    private void OnEnable()
    {
        cloneAttackButton.GetComponent<Button>().onClick.AddListener(UnlockCloneAttack);
        aggresiveAttackUnlockButton.GetComponent<Button>().onClick.AddListener(UnlockAggresiveClone);
        multipleUnlockButton.GetComponent<Button>().onClick.AddListener(UnlockMultiClone);
        crystalUnlockButton.GetComponent<Button>().onClick.AddListener(UnlockCrystal);
    }

    private void OnDisable()
    {
        cloneAttackButton.GetComponent<Button>().onClick.RemoveListener(UnlockCloneAttack);
        aggresiveAttackUnlockButton.GetComponent<Button>().onClick.RemoveListener(UnlockAggresiveClone);
        multipleUnlockButton.GetComponent<Button>().onClick.RemoveListener(UnlockMultiClone);
        crystalUnlockButton.GetComponent<Button>().onClick.RemoveListener(UnlockCrystal);
    }

    #region 解锁区域

    //解锁克隆
    private void UnlockCloneAttack()
    {
        if (cloneAttackButton.unlocked)
        {
            canAttack = true;
            attackMultipler = cloneAttackMultiplier;
        }
    }

    //解锁侵略性克隆
    private void UnlockAggresiveClone()
    {
        if (aggresiveAttackUnlockButton.unlocked)
        {
            canApplyOnHitEffect = true;
            attackMultipler = aggresiveCloneAttackMultiplier;
        }
    }

    //多重克隆
    private void UnlockMultiClone()
    {
        if (multipleUnlockButton.unlocked)
        {
            canDuplicateClone = true;
            attackMultipler = multipleCloneAttackMultiplier;
        }
    }

    private void UnlockCrystal()
    {
        if (crystalUnlockButton.unlocked)
        {
            crystalInseadOfClone = true;
        }
    }
    
    #endregion
    
    
    //创建克隆体--传递位置
    public void CreateClone(Transform clonePosition,Vector3 offset)
    {
        if (crystalInseadOfClone)
        {
            //创建水晶
            SkillManager.instance.crystal.CreateCrystal();
            return;
        }
        
        GameObject newClone = Instantiate(clonePrefab);
        //在指定位置生成
        newClone.GetComponent<CloneSkillControler>().SetupClone(clonePosition,cloneDuration,canAttack,offset,FindClosestEnemy(newClone.transform),canDuplicateClone,chanceToDuplicate,player,attackMultipler);
    }
    
    //在反击时创建克隆
    public void CreateCloneWithDelay(Transform _enemyTransform)
    {
        StartCoroutine(CloneDelayCorotine(_enemyTransform, new Vector3(1 * player.facingDir, -1.1f)));
    }

    //携程-创建克隆延迟
    private IEnumerator CloneDelayCorotine(Transform _transform,Vector3 offset)
    {
        yield return new WaitForSeconds(.4f);
        CreateClone(_transform,offset);
    }
}
