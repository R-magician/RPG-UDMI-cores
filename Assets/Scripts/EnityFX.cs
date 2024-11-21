using System;
using System.Collections;
using Cinemachine;
using UnityEngine;
using UnityEngine.Serialization;

public class EnityFX : MonoBehaviour
{
    //精灵渲染器
    private SpriteRenderer sr;
    private Player player;

    [Header("相机抖动")]
    private CinemachineImpulseSource screenShake;
    //抖动幅度
    [SerializeField] private float shakeMultiplier;
    //回收剑抖动力
    public Vector3 shakeSwordImpact;
    //高频伤害
    public Vector3 shakeHigdamage;
    

    [Header("残影")]
    //残影冷却时间
    [SerializeField] private GameObject afterImagePrefab;
    //丢失率
    [SerializeField] private float colorloseRate;
    [SerializeField] private float afterImageCooldown;
    private float afterImageCooldownTimer;
    
    
    [Header("动画特效")]
    //默认材质
    private Material orignalMat;
    //闪烁时长
    [SerializeField] private float flashDuration;
    //受伤材质
    [SerializeField] private Material hitMat;
    
    [Header("Debuff颜色")]
    //冰冻颜色
    [SerializeField] private Color[] chillColors;
    //点燃颜色
    [SerializeField] private Color[] igniteColors;
    //雷电颜色
    [SerializeField] private Color[] shockColor;

    [Header("粒子效果")]
    //火
    [SerializeField] private ParticleSystem igniteFx;
    //冰
    [SerializeField] private ParticleSystem chillFx;
    //电
    [SerializeField] private ParticleSystem shockFx;
    

    private void Start()
    {
        sr = GetComponentInChildren<SpriteRenderer>();
        player = PlayerManager.instance.Player;
        screenShake = GetComponent<CinemachineImpulseSource>();
        //获取材质
        orignalMat = sr.material;
    }

    private void Update()
    {
        afterImageCooldownTimer -= Time.deltaTime;
    }

    public void ScreenShake(Vector3 _shakePower)
    {
        screenShake.m_DefaultVelocity = new Vector3(_shakePower.x * player.facingDir, _shakePower.y) * shakeMultiplier;
        screenShake.GenerateImpulse();
    }

    //残影
    public void CreateAfterImage()
    {
        if (afterImageCooldownTimer < 0)
        {
            afterImageCooldownTimer = afterImageCooldown;
            //创建预制体
            GameObject newAfterImage = Instantiate(afterImagePrefab, transform.position, transform.rotation);
            newAfterImage.GetComponent<AfterImageFX>().SetupAfterImgae(colorloseRate,sr.sprite);
        }
       
    }
    
    //设置透明
    public void MakeTransparent(bool _transparent)
    {
        if (_transparent)
        {
            sr.color = Color.clear;
        }
        else
        {
            sr.color = Color.white;
        }
    }

    //携程动画特效--
    private IEnumerator FlashFx()
    {
        sr.material = hitMat;
        Color currentColor = sr.color;
        sr.color = Color.white;
        //等待两秒执行
        yield return new WaitForSeconds(flashDuration);
        sr.color = currentColor;
        sr.material = orignalMat;
    }

    //红色闪烁
    private void RedColorBlink()
    {
        if (sr.color != Color.white)
        {
            sr.color = Color.white;
        }
        else
        {
            sr.color  = Color.red;
        }
    }

    //关闭红色
    private void CancelColorChange()
    {
        CancelInvoke();
        sr.color = Color.white;
        igniteFx.Stop();
        chillFx.Stop();
        shockFx.Stop();
    }

    //重复点燃特效
    public void IgniteFxFor(float _seconds)
    {
        igniteFx.Play();
        //从0s开始每0.3s执行一次
        InvokeRepeating("IgniteColorFx",0,.2f);
        //_seconds秒后恢复成原本的颜色
        Invoke("CancelColorChange",_seconds);
    }
    
    //冰冻特效
    public void ChillFxFor(float _seconds)
    {
        chillFx.Play();
        //从0s开始每0.3s执行一次
        InvokeRepeating("ChillColorFx",0,.2f);
        //_seconds秒后恢复成原本的颜色
        Invoke("CancelColorChange",_seconds);
    }
    
    //雷电特效
    public void ShockFxFor(float _seconds)
    {
        shockFx.Play();
        //从0s开始每0.3s执行一次
        InvokeRepeating("ShockColorFx",0,.2f);
        //_seconds秒后恢复成原本的颜色
        Invoke("CancelColorChange",_seconds);
    }
    
    //冰冻颜色特效
    private void ChillColorFx()
    {
        //在两种颜色中切换
        if (sr.color != chillColors[0])
        {
            sr.color = chillColors[0];
        }
        else
        {
            sr.color = chillColors[1];
        }
    } 
    
    //点燃颜色特效
    private void IgniteColorFx()
    {
        //在两种颜色中切换
        if (sr.color != igniteColors[0])
        {
            sr.color = igniteColors[0];
        }
        else
        {
            sr.color = igniteColors[1];
        }
    } 
    
    //雷电颜色特效
    private void ShockColorFx()
    {
        //在两种颜色中切换
        if (sr.color != shockColor[0])
        {
            sr.color = shockColor[0];
        }
        else
        {
            sr.color = shockColor[1];
        }
    }
}
