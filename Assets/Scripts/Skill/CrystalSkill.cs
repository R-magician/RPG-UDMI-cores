//水晶技能

using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CrystalSkill : Skill
{
    //水晶持续时间
    [SerializeField] private float crystalDuration;
    //水晶预制体
    [SerializeField] private GameObject crystalPrefab;
    //当前水晶
    private GameObject currentCrystal;
    
    [Header("基础水晶")]
    [SerializeField] private UI_SkillTreeSlot unlockCrystalButton;
    [field: SerializeField] public bool crystalUnlocked{get; private set;}
    
    //克隆代替水晶
    [Header("水晶克隆")] 
    [SerializeField] private UI_SkillTreeSlot unlockCloneInstaedButton;
    [SerializeField] private bool cloneInsteadOfCrystal;
    
    [Header("爆炸水晶")]
    [SerializeField] private UI_SkillTreeSlot unlockExplosiveButton;
    //能爆炸
    [SerializeField] private bool canExplode;
    //增长速度
    [SerializeField] private float growSpeed;
    
    [Header("移动水晶")]
    [SerializeField] private UI_SkillTreeSlot unlockMovingCrystalButton;
    //能移动
    [SerializeField] private bool canMoveToEnemy;
    //移向敌人速度
    [SerializeField] private float moveSpeed;

    [Header("多个晶体")]
    [SerializeField] private UI_SkillTreeSlot unlockMultiStackButton;
    //能使用多个栈
    [SerializeField] private bool canUseMultiStacks;
    //晶体数量
    [SerializeField] private int amountOfStacks;
    //冷却时间
    [SerializeField] private float multiStackCooldown;
    //使用时间窗口
    [SerializeField] private float useTimeWindow;
    //水晶列表
    [SerializeField] private List<GameObject> crystalLeft = new List<GameObject>();


    protected override void Start()
    {
        base.Start();
        unlockCrystalButton.GetComponent<Button>().onClick.AddListener(UnlockCrystal);
        unlockCloneInstaedButton.GetComponent<Button>().onClick.AddListener(UnlockCrystalMirage);
        unlockExplosiveButton.GetComponent<Button>().onClick.AddListener(UnlockExplosiveCrystal);
        unlockMovingCrystalButton.GetComponent<Button>().onClick.AddListener(UnlockMoveingCrystal);
        unlockMultiStackButton.GetComponent<Button>().onClick.AddListener(UnlockMultiStack);
    }

    #region 解锁区域

    protected override void CheckUnlock()
    {
        UnlockCrystal();
        UnlockCrystalMirage();
        UnlockMoveingCrystal();
        UnlockExplosiveCrystal();
        UnlockMultiStack();
    }

    //解锁基础水晶
    private void UnlockCrystal()
    {
        if (unlockCrystalButton.unlocked)
        {
            crystalUnlocked = true;
        }
    }

    //解锁克隆代替水晶
    private void UnlockCrystalMirage()
    {
        if (unlockCloneInstaedButton.unlocked)
        {
            cloneInsteadOfCrystal = true;
        }  
    }

    //爆炸水晶
    private void UnlockExplosiveCrystal()
    {
        if (unlockExplosiveButton.unlocked)
        {
            canExplode = true;
        }
    }

    //移动水晶
    private void UnlockMoveingCrystal()
    {
        if (unlockMovingCrystalButton.unlocked)
        {
            canMoveToEnemy = true;
        }
    }

    //多个晶体
    private void UnlockMultiStack()
    {
        if (unlockMultiStackButton.unlocked)
        {
            canUseMultiStacks = true;
        }
    }

    #endregion

    public override void UseSkill()
    {
        base.UseSkill();
        if (canUseMultiCrystal())
        {
            return;
        }

        if (currentCrystal == null)
        {
            //创建水晶
            CreateCrystal();
        }
        else
        {
            //水晶能移动，就不能交换位置
            if (canMoveToEnemy)
            {
                return;
            }
            Vector2 playerPos = player.transform.position+new Vector3(0,1.1f);
            //当前水晶位置赋值给玩家
            player.transform.position = currentCrystal.transform.position-new Vector3(0,1.1f,0);
            currentCrystal.transform.position = playerPos;

            if (cloneInsteadOfCrystal)
            {
                //水晶变为克隆体
                SkillManager.instance.clone.CreateClone(currentCrystal.transform,new Vector3(0,-1.1f));
                //删除水晶
                Destroy(currentCrystal);
            }
            else
            {
                currentCrystal.GetComponent<CrystalSkillController>()?.FinishCrystal();
            }
        }
    }

    public void CreateCrystal()
    {
        //生成一个水晶
        currentCrystal = Instantiate(crystalPrefab,player.transform.position+new Vector3(0,1.1f,0),Quaternion.identity);
        CrystalSkillController crystalSkillController = currentCrystal.GetComponent<CrystalSkillController>();
        crystalSkillController.SetupCrystalSkill(crystalDuration,canExplode,canMoveToEnemy, moveSpeed,growSpeed,FindClosestEnemy(currentCrystal.transform),player);
    }

    public void CurrentCrystalChooseRandomTarget()
    {
        //旋转随机的敌人
        currentCrystal.GetComponent<CrystalSkillController>().ChooseRandomEnemy();
    }

    //可以使用多个晶体
    private bool canUseMultiCrystal()
    {
        if (canUseMultiStacks)
        {
            if (crystalLeft.Count > 0)
            {
                //使用水晶的时候剩下的数量和堆积的数量一致
                //在按下的那个时候就开始进行2.5s的判断，若数量一样，技能进入冷却
                if (crystalLeft.Count == amountOfStacks)
                {
                    //几秒后关闭窗口
                    Invoke("ResetAbility", useTimeWindow);
                }
                
                cooldown = 0;
                //拿到后面一个
                GameObject crystalToSpawn = crystalLeft[crystalLeft.Count-1];
                //生成对象
                GameObject newCrystal = Instantiate(crystalToSpawn,player.transform.position+new Vector3(0,1.1f,0),Quaternion.identity);
                //移出集合
                crystalLeft.Remove(crystalToSpawn);
                
                //设置水晶技能
                newCrystal.GetComponent<CrystalSkillController>()?.
                    SetupCrystalSkill(crystalDuration,canExplode,canMoveToEnemy,moveSpeed,growSpeed,FindClosestEnemy(newCrystal.transform),player);
                
                if (crystalLeft.Count <= 0)
                {
                    //冷却技能补充水晶
                    cooldown = multiStackCooldown;
                    RefilCrystal();
                }
                return true;
            }
        }
        return false;
    }
    
    //重新填充水晶
    private void RefilCrystal()
    {
        int amountToAdd = amountOfStacks - crystalLeft.Count;
        for (int i = 0; i < amountToAdd; i++)
        {
            crystalLeft.Add(crystalPrefab);
        }
    }

    //重置
    private void ResetAbility()
    {
        if (cooldownTimer > 0)
        {
            return;
        }
        cooldownTimer = multiStackCooldown;
        RefilCrystal();
    }
}
