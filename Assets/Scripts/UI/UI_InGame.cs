//用户界面

using System;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class UI_InGame : MonoBehaviour
{
    [SerializeField] private PlayerStats playerStats;
    [SerializeField] private Slider slider;

    //冲刺冷却
    [SerializeField] private Image dashImage;
    //格挡冷却
    [SerializeField] private Image parryImage;
    //水晶冷却
    [SerializeField] private Image crystalImage;
    //飞剑
    [SerializeField] private Image swordImage;
    //黑洞
    [SerializeField] private Image blackholeImage;
    //药瓶
    [SerializeField] private Image flaskImage;
    //当前灵魂
    [SerializeField] private TextMeshProUGUI currentSouls;

    private SkillManager skills;
    
    private void Start()
    {
        if (playerStats != null)
        {
            //订阅血量更新事件
            // playerStats.onHealthChanged += UpdateHealthUI;
        }
        skills = SkillManager.instance;
        
        //冲刺
        InputManager.instance.inputControl.Player.Dash.started += Dash;
        //格挡
        InputManager.instance.inputControl.Player.CounterAttack.started += CounterAttack;
        //水晶
        InputManager.instance.inputControl.Player.Crystal.started += Crystal;
        //飞剑
        InputManager.instance.inputControl.Player.ViceSkill.canceled += ViceSkill;
        //黑洞
        InputManager.instance.inputControl.Player.Blackhole.started += Blackhole;
        //使用药瓶
        InputManager.instance.inputControl.Item.UseCure.started += UseCure;
    }

    private void Update()
    {
        currentSouls.text = PlayerManager.instance.GetCurrency().ToString("#,#");
        
        //检查冲刺冷却
        CheckCooldownOf(dashImage, skills.dash.cooldown);
        //检查反击冷却
        CheckCooldownOf(parryImage, skills.parry.cooldown);
        //检查水晶冷却
        CheckCooldownOf(crystalImage, skills.crystal.cooldown);
        //检查飞剑冷却
        CheckCooldownOf(swordImage, skills.sword.cooldown);
        //检查黑洞冷却
        CheckCooldownOf(blackholeImage, skills.blackhole.cooldown);
        //检查药瓶冷却
        CheckCooldownOf(flaskImage,Inventory.instance.flaskCooldown);
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
        if (skills.dash.dashUnlocked)
        {
            SetCoolDownOf(dashImage);
        }
    }
    
    //格挡
    private void CounterAttack(InputAction.CallbackContext obj)
    {
        if (skills.parry.parryUnlocked)
        {
            SetCoolDownOf(parryImage);
        }
    }
    
    //水晶
    private void Crystal(InputAction.CallbackContext obj)
    {
        if (skills.crystal.crystalUnlocked)
        {
            SetCoolDownOf(crystalImage);
        }
    }
    
    //飞剑
    private void ViceSkill(InputAction.CallbackContext obj)
    {
        if (skills.sword.swordUnlocked)
        {
            SetCoolDownOf(swordImage);
        }
    }
    
    //黑洞
    private void Blackhole(InputAction.CallbackContext obj)
    {
        if (skills.blackhole.blackholeUnlocked)
        {
            SetCoolDownOf(blackholeImage);
        }
    }
    
    //使用药瓶
    private void UseCure(InputAction.CallbackContext obj)
    {
        if (Inventory.instance.GetEquipment(EquipmentType.Flask) !=null)
        {
            SetCoolDownOf(flaskImage);
        }
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
