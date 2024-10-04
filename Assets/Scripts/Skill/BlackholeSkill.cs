using UnityEngine;

public class BlackholeSkill : Skill
{
    //黑洞预制体
    [SerializeField] private int amountOfAttacks;
    [SerializeField] private float cloneCooldown;
    [SerializeField] private float blackholeTimer;
    [Space]
    [SerializeField] private GameObject blackholePrefab;
    [SerializeField] private float maxSize;
    [SerializeField] private float growSpeed;
    [SerializeField] private float shrinkSpeed;

    private BlackholeSkillController currentBlackholeController;
    public GameObject blackhole;
    
    protected override void Awake()
    {
        base.Awake();
    }

    protected override void Update()
    {
        base.Update();
    }

    public override bool CanUseSkill()
    {
        return base.CanUseSkill();
    }

    public override void UseSkill()
    {
        base.UseSkill();
        //若黑洞没有创建
        if (!blackhole)
        {
            //创建黑洞预制体
            blackhole = Instantiate(blackholePrefab,player.transform.position+new Vector3(0,1.1f,0),Quaternion.identity);
            //获取预制体上的控制组件
            currentBlackholeController = blackhole.GetComponent<BlackholeSkillController>();
            //创建黑洞
            currentBlackholeController.SetupBlackhole(maxSize,growSpeed,shrinkSpeed,amountOfAttacks,cloneCooldown,blackholeTimer);
        }
        
    }

    //黑洞技能是否已经完成
    public bool BlackholeCompleted()
    {
        if (!currentBlackholeController)
        {
            return false;
        }
        
        if (currentBlackholeController.playerCanExitState)
        {
            //控制器置空
            currentBlackholeController = null;
            //技能置空
            blackhole = null;
            return true;
        }
        return false;
    }
}
