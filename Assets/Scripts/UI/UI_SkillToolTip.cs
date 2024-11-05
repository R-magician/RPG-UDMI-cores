//技能面板--提神框

using TMPro;
using UnityEngine;

public class UI_SkillToolTip : UI_ToolTip
{
    [SerializeField] private TextMeshProUGUI skillText;
    [SerializeField] private TextMeshProUGUI skillName;
    [SerializeField] private TextMeshProUGUI skillCost;
    [SerializeField] private float defaultNameFontSize;  

    public void ShowToolTip(string _text,string _name,int _cost)
    {
        skillName.text = _name;
        skillText.text = _text;
        skillCost.text = "花费："+_cost.ToString();
        AdjustPosition();
        AdjustFontSize(skillName);
        gameObject.SetActive(true);
    }

    public void HideToolTip()
    {
        skillName.fontSize = defaultNameFontSize;
        gameObject.SetActive(false);
    }
}
