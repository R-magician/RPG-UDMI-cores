//黑洞热键控制

using System;
using TMPro;
using UnityEngine;

public class BlackHoleHotKeyController : MonoBehaviour
{
    private SpriteRenderer sr;
    //触发热键
    private KeyCode myHotKey;
    //显示文字
    private TextMeshProUGUI myText;
    //敌人信息
    private Transform enemiesTransform;
    //黑洞
    private BlackholeSkillController blackHole;

    public void SetupHotKey(KeyCode hotKey,Transform enemy,BlackholeSkillController myBlackHole)
    {
        sr = GetComponent<SpriteRenderer>();
        myText=GetComponentInChildren<TextMeshProUGUI>();

        enemiesTransform = enemy;
        blackHole = myBlackHole;
        
        myHotKey = hotKey;
        myText.text = hotKey.ToString();
    }

    private void Update()
    {
        if (Input.GetKeyDown(myHotKey))
        {
            //按下键盘得到相应的位置--将敌人坐标添加到集合
            blackHole.AddEnemyToList(enemiesTransform);
            
            //清除文本颜色
            myText.color = Color.clear;
            sr.color = Color.clear;
        }
    }
}
