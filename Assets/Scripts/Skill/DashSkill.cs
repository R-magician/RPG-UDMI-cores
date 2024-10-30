//冲刺技能
using UnityEngine;
using UnityEngine.UI;

public class DashSkill : Skill
{

    [Header("冲刺")] 
    //技能解锁
    public bool dashUnlocked { get;private set; }
    [SerializeField] 
    private UI_SkillTreeSlot dashUnlockButton;
    

    [Header("克隆冲刺")] 
    public bool cloneOnDashUnlocked { get;private set; }
    [SerializeField] 
    private UI_SkillTreeSlot cloneOnDashUnlockButton;

    [Header("克隆到达")] 
    public bool cloneOnArrivalsUnlocked { get;private set; }
    [SerializeField] 
    private UI_SkillTreeSlot cloneOnArrivalsUnlockButton;
        
    public override void UseSkill()
    {
        base.UseSkill();
    }

    protected override void Awake()
    {
        base.Awake();
    }

    protected override void Start()
    {
        base.Start();
        //解锁冲刺
        dashUnlockButton.GetComponent<Button>().onClick.AddListener(UnlockDash);
        cloneOnDashUnlockButton.GetComponent<Button>().onClick.AddListener(UnlockCloneOnDash);
        cloneOnArrivalsUnlockButton.GetComponent<Button>().onClick.AddListener(UnlockArrivalsOnDash);
    }

    private void UnlockDash()
    {
        if (dashUnlockButton.unlocked)
        {
            //解锁冲刺
            dashUnlocked = true;
        }
    }
    
    private void UnlockCloneOnDash()
    {
        if (cloneOnDashUnlockButton.unlocked)
        {
            cloneOnDashUnlocked = true;
        }
    }
    
    private void UnlockArrivalsOnDash()
    {
        if (cloneOnArrivalsUnlockButton.unlocked)
        {
            cloneOnArrivalsUnlocked = true;
        }
    }
    
    //在Dash开始时创建克隆
    public void CloneOnDash()
    {
        if (cloneOnDashUnlocked)
        {
            SkillManager.instance.clone.CreateClone(player.transform,Vector3.zero);
        }
    }

    //在Dash结束时创建克隆
    public void CloneOnArrival()
    {
        if (cloneOnArrivalsUnlocked)
        {
            SkillManager.instance.clone.CreateClone(player.transform,Vector3.zero);
        }
    }
}
