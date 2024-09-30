using System;
using UnityEngine;
using UnityEngine.InputSystem;

public class SwordSkill : Skill
{
    [Header("技能信息")] 
    //剑预制体
    [SerializeField] private GameObject swordPrefab;
    //发射方向的力
    [SerializeField] private Vector2 launchForce;
    //飞行重力
    [SerializeField] private float swordGravity;

    [Header("弧线点")]
    //点的数量
    [SerializeField] private int numberOfDots;
    //点与点之间的距离
    [SerializeField] private float spaceBeetwenDots;
    //点
    [SerializeField] private GameObject dotPrefab;
    //点的父级
    [SerializeField] private Transform dotsParent;

    private GameObject[] dots;
    
    //发射方向
    private Vector2 finalDir;

    protected override void Awake()
    {
        base.Awake();
        
        //抬起右鼠标键
        player.inputControl.Player.ViceSkill.canceled += ViceSkill_c;
        
        //生成点
        GenereateDots();
    }

    protected override void Update()
    {
        base.Update();
        
        //按键是否是“进行中”的状态
        if (player.inputControl.Player.ViceSkill.IsPressed())
        {
            // 按住状态，持续执行拖动或其他操作
            ViceSkill_p();
        }
    }


    private void ViceSkill_c(InputAction.CallbackContext obj)
    {
        finalDir = new Vector2(AimDirection().normalized.x * launchForce.x,AimDirection().normalized.y * launchForce.y);
    }
    
    //按住执行
    private void ViceSkill_p()
    {
        // 获取当前的鼠标位置
        for (int i = 0; i < dots.Length; i++)
        {
            //index * 点与点之间的距离
            dots[i].transform.position = DotsPosition(i * spaceBeetwenDots);
        }
    }



    //创建技能
    public void CreateSword()
    {
        //生成手里剑的复制体
        GameObject newSword = Instantiate(swordPrefab,player.transform.position,transform.rotation);
        //获得剑技能控制器的组件
        SwordSkillControler newSwordScript = newSword.GetComponent<SwordSkillControler>();
        //开始飞剑
        newSwordScript.SetupSword(finalDir,swordGravity);
        //创建好了剑，就关闭点
        DotsActive(false);
    }
    
    //目标方向
    public Vector2 AimDirection()
    {
        //起始位置
        Vector2 playerPostion = player.transform.position; 
        //瞄准的位置 --相机的位置到世界点的位置
        Vector2 mousePostiom = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        //方向 = 鼠标位置-玩家位置
        Vector2 direction = mousePostiom - playerPostion;

        return direction;
    }

    //显示隐藏点
    public void DotsActive(bool isActive)
    {
        for (int i = 0; i < numberOfDots; i++)
        {
            dots[i].SetActive(isActive);
        }
    }

    //生成点
    private void GenereateDots()
    {
        dots = new GameObject[numberOfDots];
        for (int i = 0; i < numberOfDots; i++)
        {
            dots[i] = Instantiate(dotPrefab,player.transform.position,Quaternion.identity,dotsParent);
            //不显示
            dots[i].SetActive(false);
        }
    }
    
    //设置点的位置
    private Vector2 DotsPosition(float t)
    {
        Vector2 position = (Vector2)player.transform.position + new Vector2(
            AimDirection().normalized.x * launchForce.x,
            AimDirection().normalized.y * launchForce.y) * t + .5f * (Physics2D.gravity * swordGravity) * (t * t);

        return position;
    }
}
