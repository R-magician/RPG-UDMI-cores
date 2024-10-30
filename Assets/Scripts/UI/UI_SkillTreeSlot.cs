//技能面板

using System;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class UI_SkillTreeSlot : MonoBehaviour,IPointerEnterHandler,IPointerExitHandler
{
    private UI ui;
    [SerializeField] private Image skillImage;
    
    //解锁技能需要的货币
    [SerializeField] private int skillPrice;
    
    //技能名
    [SerializeField] private string skillName;
    //技能描述
    [TextArea]
    [SerializeField] private string skillDescription;

    //锁定技能颜色
    [SerializeField] private Color lockeSkillColor;
    
    //解锁状态
    public bool unlocked;

    //技能解锁插槽
    [SerializeField] private UI_SkillTreeSlot[] shouldBeUnlocked;
    //技能被锁定插槽
    [SerializeField] private UI_SkillTreeSlot[] shouldBelocked;


    private void OnValidate()
    {
        gameObject.name = "Skill_UI - " + skillName;
    }

    private void Awake()
    {
        skillImage = GetComponent<Image>();
        skillImage.color = lockeSkillColor;
        
        //给button添加事件
        GetComponent<Button>().onClick.AddListener(()=>UnlockSkillSlot());
        ui = GetComponentInParent<UI>();
    }

    //解锁技能
    public void UnlockSkillSlot()
    {
        if (PlayerManager.instance.HaveEnoughMoney(skillPrice)==false)
        {
            //没有足够的货币，退出
            return;
        }
        
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

        skillImage.color = Color.white;
    }


    public void OnPointerEnter(PointerEventData eventData)
    {
        ui.skillToolTip.ShowToolTip(skillDescription,skillName);
        Vector2 mousePosition = Input.mousePosition;

        float xOffset = 0;
        float yOffset = 0;

        if (mousePosition.x > 600)
        {
            xOffset = -150f;
        }
        else
        {
            xOffset = 150f;
        }

        if (mousePosition.y > 320)
        {
            yOffset = -150f;
        }
        else
        {
            yOffset = 150f;
        }

        ui.skillToolTip.transform.position = new Vector2(mousePosition.x + xOffset, mousePosition.y + yOffset);
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        ui.skillToolTip.HideToolTip();
    }
}
