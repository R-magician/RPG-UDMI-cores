//用户界面

using System;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class UI_InGame : MonoBehaviour
{
    [SerializeField] private PlayerStats playerStats;
    [SerializeField] private Slider slider;

    //冲刺冷却
    [SerializeField] private Image dashImage;
    //闪避冷却
    [SerializeField] private Image parryImage;

    private SkillManager skills;
    
    private void Start()
    {
        if (playerStats != null)
        {
            //订阅血量更新事件
            // playerStats.onHealthChanged += UpdateHealthUI;
        }
        skills = SkillManager.instance;
        
        InputManager.instance.inputControl.Player.Dash.started += Dash;
    }

    private void Update()
    {
        //检查冲刺冷却
        CheckCooldownOf(dashImage, skills.dash.cooldown);
    }

    //更新血量UI
    private void UpdateHealthUI()
    {
        //当前血量=最大生命值+活力
        slider.maxValue = playerStats.GetMaxHealthValue();
        slider.value = playerStats.currentHealth;
    }

    //设置冷却
    private void SetCoolDownOf(Image _image)
    {
        if (_image.fillAmount <= 0)
        {
            _image.fillAmount = 1;
        }
    }
    
    //冲刺
    private void Dash(InputAction.CallbackContext obj)
    {
        SetCoolDownOf(dashImage);
    }

    //检测冷却时间
    private void CheckCooldownOf(Image _image,float _cooldown)
    {
        if (_image.fillAmount > 0)
        {
            _image.fillAmount -= 1 / _cooldown * Time.deltaTime;
        }
    }
}
