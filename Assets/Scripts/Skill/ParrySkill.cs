//格挡技能
using UnityEngine;
using UnityEngine.UI;

public class ParrySkill : Skill
{
    [Header("格挡反击")] 
    [SerializeField] private UI_SkillTreeSlot parryUnlockButton;
    [SerializeField] public bool parryUnlocked { get;private set; }
    
    [Header("格挡反击回血")]
    [SerializeField] private UI_SkillTreeSlot restoreUnlockButton;
    public bool restoreUnlocked { get;private set; }
    //回血量
    [Range(0f,1f)]
    [SerializeField] private float restoreHealth;
    
    [Header("格挡克隆反击")]
    [SerializeField] private UI_SkillTreeSlot parryWithMirageUnlockButton;
    public bool parryWithMirageUnlocked { get;private set; }
    
    public override void UseSkill()
    {
        base.UseSkill();

        if (restoreUnlocked)
        {
            int health  = Mathf.RoundToInt(player.stats.GetMaxHealthValue() * restoreHealth);
            player.stats.IncreaseHealthBy(health);
        }
    }

    protected override void Start()
    {
        base.Start();
        parryUnlockButton.GetComponent<Button>().onClick.AddListener(UnlockParry);
        restoreUnlockButton.GetComponent<Button>().onClick.AddListener(UnlockRestore);
        parryWithMirageUnlockButton.GetComponent<Button>().onClick.AddListener(UnlockParryWithMirage);
    }

    //解锁格挡
    private void UnlockParry()
    {
        if (parryUnlockButton.unlocked)
        {
           parryUnlocked = true;
        }
    }

    //格挡反击回血
    private void UnlockRestore()
    {
        if (restoreUnlockButton.unlocked)
        {
            restoreUnlocked = true;
        }
    }

    //解锁格挡克隆反击
    private void UnlockParryWithMirage()
    {
        if (parryWithMirageUnlockButton.unlocked)
        {
            parryWithMirageUnlocked = true;
        }
    }

    //制作幻影
    public void MakeMirageOnParry(Transform _transform)
    {
        if (parryWithMirageUnlocked)
        {
            SkillManager.instance.clone.CreateCloneWithDelay(_transform);
        }
    }
}
