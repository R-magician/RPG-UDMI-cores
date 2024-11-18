//UI脚本控制

using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;

public class UI : MonoBehaviour,ISaveManager
{
    [Header("结束画面")]
    //屏幕加载效果
    [SerializeField] private UI_FadeScreen fadeScreen;
    //结束文字
    [SerializeField] private GameObject endText;

    [SerializeField] private GameObject restartButton;
    [Space]
    
    //角色信息
    [SerializeField] private GameObject charcaterUI;
    //技能树
    [SerializeField] private GameObject skillTreeUI;
    //材料
    [SerializeField] private GameObject craftUI;
    //选项
    [SerializeField] private GameObject optionsUI;
    //玩家界面
    [SerializeField] private GameObject inGameUI;

    public UI_SkillToolTip skillToolTip;
    public UI_ItemToolTip itemToolTip;
    public UI_StatToolTip statToolTip;
    public UI_CraftWindow craftWindow;

    //音效设置
    [SerializeField] private UI_VolumeSlider[] volumeSettings;


    private void Awake()
    {
        //需要用这个来分配事件
        SwitchTo(skillTreeUI);
        fadeScreen.gameObject.SetActive(true); 
        
        itemToolTip.gameObject.SetActive(false);
        statToolTip.gameObject.SetActive(false);

        InputManager.instance.inputControl.UI.CharactPanel.started += CharactPanel;
        InputManager.instance.inputControl.UI.CraftPanel.started += CraftPanel;
        InputManager.instance.inputControl.UI.SkillPanel.started += SkillPanel;
        InputManager.instance.inputControl.UI.OptionPanel.started += OptionPanel;
    }

    private void Start()
    {
        SwitchTo(inGameUI);
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
            //场景过渡是否使用淡入淡出
            bool isFadeScreen = transform.GetChild(i).GetComponent<UI_FadeScreen>() != null;
            
            if (isFadeScreen==false)
            {
                //先关闭显示所有面板显示
                transform.GetChild(i).gameObject.SetActive(false);
            }
            
        }

        if (_menu != null)
        {
            AudioManager.instance.PlaySFX(8,null);
            //显示
            _menu.SetActive(true);
        }

        if (GameManager.instance != null)
        {
            if (_menu == inGameUI)
            {
                GameManager.instance.PauseGame(false);
            }
            else
            {
                GameManager.instance.PauseGame(true);
            }
        }
    }

    //按键开关
    public void SwitchWithKeyTo(GameObject _menu)
    {
        //activeSelf,判断组件是否激活
        if (_menu != null && _menu.activeSelf)
        {
            _menu.SetActive(false);
            CheckForInGameUI();
            return;
        }
        
        SwitchTo(_menu);
    }

    //检查玩家UI
    private void CheckForInGameUI()
    {
        for (int i = 0; i < transform.childCount; i++)
        {
            if (transform.GetChild(i).gameObject.activeSelf && transform.GetChild(i).GetComponent<UI_FadeScreen>() ==null)
            {
                return;
            }
        }

        SwitchTo(inGameUI);
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

    //打开结束屏幕
    public void SwitchOnEndScreen()
    {
        fadeScreen.FadeOut();
        StartCoroutine(EndScreenCorutione());
    }

    IEnumerator EndScreenCorutione()
    {
        yield return new WaitForSeconds(1f);
        endText.SetActive(true);
        yield return new WaitForSeconds(1.5f);
        restartButton.SetActive(true);
    }

    //重新开始按钮
    public void RestartGameButton()
    {
        GameManager.instance.RestartScene();
    }
    
    public void LoadData(GameData _data)
    {
        foreach (KeyValuePair<string,float> pair in _data.volumeSettings)
        {
            foreach (UI_VolumeSlider item in volumeSettings)
            {
                if (item.parametr == pair.Key)
                {
                    item.LoadSlider(pair.Value);
                }
            }
        }
    }

    
    public void SaveData(ref GameData _data)
    {
        _data.volumeSettings.Clear();
        foreach (UI_VolumeSlider item in volumeSettings)
        {
            _data.volumeSettings.Add(item.parametr,item.slider.value);
        }
    }
}
