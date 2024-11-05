//物品数据-装备

using System.Collections.Generic;
using UnityEngine;

public enum EquipmentType
{
    Weapon,//武器
    Armor,//盔甲
    Amulet,//项链-护身符
    Flask,//瓶子
}

[CreateAssetMenu(fileName = "新物品数据",menuName = "数据/装备")]
public class ItemDataEquipment : ItemData
{
    //装备类型
    public EquipmentType equipmentType;

    [Header("独特的效果")]
    //物品使用冷却
    public float itemCooldown;
    //物品特效
    public ItemEffect[] itemEffects;
    
    [TextArea]
    public string itemEffectDescription;
    
    [Header("主要数值")]
    //力量
    public int strength;
    //敏捷
    public int agility;
    //智力
    public int intelgenace;
    //活力
    public int vitality;

    [Header("攻击数值")] 
    //攻击力
    public int damage;
    //暴击率
    public int critChance;
    //暴击值
    public int critPower;

    [Header("防御数值")]
    //生命值
    public int health;
    //护甲
    public int armor;
    //闪避值
    public int evasion;
    //魔抗值
    public int magicResistance;

    [Header("魔法数值")]
    //火焰伤害
    public int fireDamage;
    //冰冻伤害
    public int iceDamage;
    //雷电伤害
    public int lightingDamage;
    
    [Header("工艺要求")]
    public List<InventoryItem> craftingMaterials;

    //描述长度
    private int descriptionLength;
    
    

    //物品特效
    public void Effect(Transform _enemyPosition)
    {
        foreach (var item in itemEffects)
        {
            //执行特效
            item.ExecuteEffect(_enemyPosition);
        }
    }
    
    //添加修改器
    public void AddModifiers()
    {
        //获取玩家数值
        PlayerStats playerStats = PlayerManager.instance.Player.GetComponent<PlayerStats>();
        
        //添加到数值修改器中
        playerStats.strength.AddModifier(strength);
        playerStats.agility.AddModifier(agility);
        playerStats.intelligence.AddModifier(intelgenace);
        playerStats.vitality.AddModifier(vitality);
        
        playerStats.damage.AddModifier(damage);
        playerStats.critChance.AddModifier(critChance);
        playerStats.critPower.AddModifier(critPower);
        
        playerStats.maxHealth.AddModifier(health);
        playerStats.armor.AddModifier(armor);
        playerStats.evasion.AddModifier(evasion);
        playerStats.magicResistance.AddModifier(magicResistance);
        
        playerStats.fireDamage.AddModifier(fireDamage);
        playerStats.iceDamage.AddModifier(iceDamage);
        playerStats.lightningDamage.AddModifier(lightingDamage);
    }

    //移除修改器
    public void RemoveModifiers()
    {
        //获取玩家数值
        PlayerStats playerStats = PlayerManager.instance.Player.GetComponent<PlayerStats>();
        
        //添加到数值修改器中
        playerStats.strength.RemoveModifier(strength);
        playerStats.agility.RemoveModifier(agility);
        playerStats.intelligence.RemoveModifier(intelgenace);
        playerStats.vitality.RemoveModifier(vitality);
        
        playerStats.damage.RemoveModifier(damage);
        playerStats.critChance.RemoveModifier(critChance);
        playerStats.critPower.RemoveModifier(critPower);
        
        playerStats.maxHealth.RemoveModifier(health);
        playerStats.armor.RemoveModifier(armor);
        playerStats.evasion.RemoveModifier(evasion);
        playerStats.magicResistance.RemoveModifier(magicResistance);
        
        playerStats.fireDamage.RemoveModifier(fireDamage);
        playerStats.iceDamage.RemoveModifier(iceDamage);
        playerStats.lightningDamage.RemoveModifier(lightingDamage);
    }

    public override string GetDescription()
    {
        sb.Length = 0;
        descriptionLength = 0;
        AddItemDescription(strength,"攻击力");
        AddItemDescription(agility,"敏捷");
        AddItemDescription(intelgenace,"智力");
        AddItemDescription(vitality,"活力");
        
        AddItemDescription(damage,"基础伤害");
        AddItemDescription(critChance,"基础暴击率");
        AddItemDescription(critPower,"基础暴击伤害");
        AddItemDescription(health,"最大血量");
        
        AddItemDescription(armor,"基础护甲");
        AddItemDescription(evasion,"基础闪避");
        AddItemDescription(magicResistance,"魔法抗性");
        
        AddItemDescription(fireDamage,"火焰伤害");
        AddItemDescription(fireDamage,"冰冻伤害");
        AddItemDescription(fireDamage,"雷电伤害");

        for (int i = 0; i < itemEffects.Length; i++)
        {
            if (itemEffects[i].effectDescription.Length > 0)
            {
                sb.AppendLine();
                sb.Append("效果描述： "+itemEffects[i].effectDescription);
                descriptionLength++;
            }
        }

        if (descriptionLength < 5)
        {
            for (int i = 0; i < 5-descriptionLength; i++)
            {
                sb.AppendLine();
                sb.Append("");
            }
        }
        
        return sb.ToString();
    }

    //添加物品描述
    private void AddItemDescription(int _value,string _name)
    {
        if (_value != 0)
        {
            if (sb.Length > 0)
            {
                sb.AppendLine();
            }

            if (_value > 0)
            {
                sb.Append("+ " + _value+" "+_name);
            }

            descriptionLength++;
        }
    }
}
