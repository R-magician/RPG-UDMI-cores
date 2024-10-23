//UI脚本控制

using System;
using UnityEngine;

public class UI : MonoBehaviour
{
    [SerializeField] private GameObject charcaterUI;

    public UI_ItemToolTip itemToolTip;
    

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
}
