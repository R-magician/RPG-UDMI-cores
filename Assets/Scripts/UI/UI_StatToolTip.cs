//属性工具提示

using TMPro;
using UnityEngine;

public class UI_StatToolTip : UI_ToolTip
{
    //描述
    [SerializeField] private TextMeshProUGUI description;

    //显示提示
    public void ShowStatToolTip(string _text)
    {
        description.text = _text;
        AdjustPosition();
        
        gameObject.SetActive(true);
    }

    public void HideStatToolTip()
    {
        description.text = "";
        gameObject.SetActive(false);
    }
}
