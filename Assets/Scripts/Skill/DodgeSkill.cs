//闪避
using UnityEngine;
using UnityEngine.UI;

public class DodgeSkill : Skill
{
    [Header("闪避")] 
    [SerializeField] private UI_SkillTreeSlot unlockDodgeButton;
    //闪避值
    [SerializeField] private int evasionAmount;
    public bool dogeUnlocked;
    
    [Header("闪避克隆")]
    [SerializeField] private UI_SkillTreeSlot unlockMirageDodgeButton;
    public bool dogeMirageUnlocked;

    protected override void Start()
    {
        base.Start();
        unlockDodgeButton.GetComponent<Button>().onClick.AddListener(UnlocDodge);
        unlockMirageDodgeButton.GetComponent<Button>().onClick.AddListener(UnlocMirageDodge);
    }

    private void UnlocDodge()
    {
        if (unlockDodgeButton.unlocked)
        {
            player.stats.evasion.AddModifier(evasionAmount);
            //更新UI上的统计数值
            Inventory.instance.UpdateStatsUI();
            dogeUnlocked = true;
        }   
    }
    
    private void UnlocMirageDodge()
    {
        if (unlockMirageDodgeButton.unlocked)
        {
            dogeMirageUnlocked = true;
        }   
    }

    //创建闪避克隆
    public void CreateMirageOnDoage()
    {
        if (dogeMirageUnlocked)
        {
            SkillManager.instance.clone.CreateClone(player.transform,new Vector3(2 * player.facingDir,0));
        }
    }
}
