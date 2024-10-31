//UI脚本控制

using System;
using UnityEngine;
using UnityEngine.InputSystem;

public class UI : MonoBehaviour
{
    //角色信息
    [SerializeField] private GameObject charcaterUI;
    //技能树
    [SerializeField] private GameObject skillTreeUI;
    //材料
    [SerializeField] private GameObject craftUI;
    //选项
    [SerializeField] private GameObject optionsUI;

    public UI_SkillToolTip skillToolTip;
    public UI_ItemToolTip itemToolTip;
    public UI_StatToolTip statToolTip;
    public UI_CraftWindow craftWindow;


    private void Awake()
    {
        //需要用这个来分配事件
        SwitchTo(skillTreeUI);
        
        itemToolTip.gameObject.SetActive(false);
        statToolTip.gameObject.SetActive(false);

        InputManager.instance.inputControl.UI.CharactPanel.started += CharactPanel;
        InputManager.instance.inputControl.UI.CraftPanel.started += CraftPanel;
        InputManager.instance.inputControl.UI.SkillPanel.started += SkillPanel;
        InputManager.instance.inputControl.UI.OptionPanel.started += OptionPanel;
    }

    private void Start()
    {
        SwitchTo(null);
    }

    private void OnDisable()
    {
        InputManager.instance.inputControl.UI.CharactPanel.started -= CharactPanel;
        InputManager.instance.inputControl.UI.CraftPanel.started -= CraftPanel;
        InputManager.instance.inputControl.UI.SkillPanel.started -= SkillPanel;
        InputManager.instance.inputControl.UI.OptionPanel.started -= OptionPanel;
    }


    //切换面板
    public void SwitchTo(GameObject _menu)
    {
        for (int i = 0; i < transform.childCount; i++)
        {
            //先关闭显示所有面板显示
            transform.GetChild(i).gameObject.SetActive(false);
        }

        if (_menu != null)
        {
            //显示
            _menu.SetActive(true);
        }
    }

    //按键开关
    public void SwitchWithKeyTo(GameObject _menu)
    {
        //activeSelf,判断组件是否激活
        if (_menu != null && _menu.activeSelf)
        {
            _menu.SetActive(false);
            return;
        }
        
        SwitchTo(_menu);
    }
    
    //创建选项面板
    private void OptionPanel(InputAction.CallbackContext obj)
    {
        SwitchWithKeyTo(optionsUI);
    }

    //创建技能树面板
    private void SkillPanel(InputAction.CallbackContext obj)
    {
        SwitchWithKeyTo(skillTreeUI);
    }

    //材料面板
    private void CraftPanel(InputAction.CallbackContext obj)
    {
        SwitchWithKeyTo(craftUI);
    }

    //角色信息面板
    private void CharactPanel(InputAction.CallbackContext obj)
    {
        SwitchWithKeyTo(charcaterUI);
    }
}
