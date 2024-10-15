using System;
using UnityEngine;
using UnityEngine.InputSystem;

//飞剑类型
public enum SwordType
{
    Regular,//常规
    Bounce,//弹跳
    Pierce,//穿透
    Spin,//旋转滞空
}

public class SwordSkill : Skill
{
    
    public SwordType swordType = SwordType.Regular;
    
    [Header("弹跳相关")] 
    //反弹次数
    [SerializeField] private int bounceAmount;
    //反弹重力
    [SerializeField] private float bounceGravity;
    //弹跳速度
    [SerializeField] private float bounceSpeed;
    
    [Header("穿透信息")]
    //穿透数量
    [SerializeField] private int pierceAmount;
    //穿透重力
    [SerializeField] private float pierceGravity;

    [Header("旋转信息")]
    //伤害冷却
    [SerializeField] private float hitCooldown=0.35f;
    //最大移动距离
    [SerializeField] private float maxTravelDistance=7;
    //旋转时间
    [SerializeField] private float spinDuration =2;
    //旋转重力
    [SerializeField] private float spinGravity=1;
    
    [Header("技能信息")] 
    //剑预制体
    [SerializeField] private GameObject swordPrefab;
    //发射方向的力
    [SerializeField] private Vector2 launchForce;
    //飞行重力
    [SerializeField] private float swordGravity;
    //冻结时间
    [SerializeField] private float freezeTimeDuration;
    //飞剑返回速度
    [SerializeField] private float returnSpeed;

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
        InputManager.instance.inputControl.Player.ViceSkill.canceled += ViceSkill_c;
        
        //生成点
        GenereateDots();
        
        //设置重力
        SetupGravity();
    }

    //设置重力
    private void SetupGravity()
    {
        //飞剑类型是穿透
        if (swordType == SwordType.Bounce)
        {
            //弹跳重力
            swordGravity = bounceGravity;
        }else if (swordType == SwordType.Pierce)
        {
            //穿透重力
            swordGravity = pierceGravity;
        }else if (swordType == SwordType.Spin)
        {
            //旋转重力
            swordGravity = spinGravity;
        }
    }

    protected override void Update()
    {
        base.Update();
        
        //按键是否是“进行中”的状态
        if (InputManager.instance.inputControl.Player.ViceSkill.IsPressed())
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
        GameObject newSword = Instantiate(swordPrefab, (player.transform.position)+new Vector3(.5f*player.facingDir,1f,0),transform.rotation);
        //获得剑技能控制器的组件
        SwordSkillControler newSwordScript = newSword.GetComponent<SwordSkillControler>();

        if (swordType == SwordType.Bounce)
        {
            //飞剑类型是弹跳
            newSwordScript.SetupBounce(true,bounceAmount,bounceSpeed);
        }else if (swordType == SwordType.Pierce)
        {
            //飞剑类型是穿透
            newSwordScript.SetupPierce(pierceAmount);
        }else if (swordType == SwordType.Spin)
        {
            //飞剑类型是旋转
            newSwordScript.SetupSpin(true,maxTravelDistance,spinDuration,hitCooldown);
        }
        //开始飞剑
        newSwordScript.SetupSword(finalDir,swordGravity,player,freezeTimeDuration,returnSpeed);
        player.AssignNewSword(newSword);
        //创建好了剑，就关闭点
        DotsActive(false);
    }

    #region 瞄准区域
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
            dots[i] = Instantiate(dotPrefab,player.transform.position+new Vector3(.5f*player.facingDir,1f,0),Quaternion.identity,dotsParent);
            //不显示
            dots[i].SetActive(false);
        }
    }
    
    //设置点的位置
    private Vector2 DotsPosition(float t)
    {
        Vector2 position = (Vector2)player.transform.position+new Vector2(.5f*player.facingDir,1f) + new Vector2(
            AimDirection().normalized.x * launchForce.x,
            AimDirection().normalized.y * launchForce.y) * t + .5f * (Physics2D.gravity * swordGravity) * (t * t);

        return position;
    }
    #endregion
}
