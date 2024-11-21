//图片残影

using System;
using UnityEngine;

public class AfterImageFX : MonoBehaviour
{
    private SpriteRenderer sr;
    
    private float colorlooseRate;

    public void SetupAfterImgae(float _loosingSpeed, Sprite _spriteImage)
    {
        sr = GetComponent<SpriteRenderer>();
        sr.sprite = _spriteImage;
        colorlooseRate = _loosingSpeed;
    }

    private void Update()
    {
        float alpha = sr.color.a - colorlooseRate * Time.deltaTime;
        //设置透明度
        sr.color = new Color(sr.color.r, sr.color.g, sr.color.b, alpha);

        if (sr.color.a <=0)
        {
            Destroy(gameObject);
        }
    }
}
