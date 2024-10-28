//技能面板

using System;
using UnityEngine;
using UnityEngine.UI;

public class UI_SkillTreeSlot : MonoBehaviour
{
    //解锁状态
    public bool unlocked;

    //技能解锁插槽
    [SerializeField] private UI_SkillTreeSlot[] shouldBeUnlocked;
    //技能被锁定插槽
    [SerializeField] private UI_SkillTreeSlot[] shouldBelocked;

    [SerializeField] private Image skillImage;
    
    private void Awake()
    {
        skillImage = GetComponent<Image>();
        skillImage.color = Color.red;
        
        //给button添加事件
        GetComponent<Button>().onClick.AddListener(()=>UnlockSkillSlot());
    }

    //解锁技能
    public void UnlockSkillSlot()
    {
        for (int i = 0; i < shouldBeUnlocked.Length; i++)
        {
            if (shouldBeUnlocked[i].unlocked == false)
            {
                //无法解锁技能
                return;
            }
        }
        
        for (int i = 0; i < shouldBelocked.Length; i++)
        {
            if (shouldBelocked[i].unlocked == true)
            {
                //解锁技能
                return;
            }
        }

        unlocked = true;

        skillImage.color = Color.green;
    }
    
    
}
