using System;
using System.Collections;
using UnityEngine;

public class EnityFX : MonoBehaviour
{
    //精灵渲染器
    private SpriteRenderer sr;
    
    [Header("动画特效")]
    //默认材质
    private Material orignalMat;
    //闪烁时长
    [SerializeField] private float flashDuration;
    //受伤材质
    [SerializeField] private Material hitMat;

    private void Start()
    {
        sr = GetComponentInChildren<SpriteRenderer>();
        //获取材质
        orignalMat = sr.material;
    }

    //携程动画特效--
    private IEnumerator FlashFx()
    {
        sr.material = hitMat;
        //等待两秒执行
        yield return new WaitForSeconds(flashDuration);
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
    private void CancelRedBlink()
    {
        CancelInvoke();
        sr.color = Color.white;
    }
}
