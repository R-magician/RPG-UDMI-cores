//视查背景--随着摄像机移动背景移动

using System;
using UnityEngine;

public class ParallaxBackground : MonoBehaviour
{
    //主相机
    private GameObject cam;

    //视差效果
    [SerializeField] private float parallaxEffect;

    //当前组件的位置
    private float xPosition;
    
    //获取SpriteRenderer的宽度
    private float length;
    
    private void Start()
    {
        cam = GameObject.Find("Main Camera");

        //获取SpriteRenderer的宽度
        length = GetComponent<SpriteRenderer>().bounds.size.x;
        xPosition = transform.position.x;
    }

    private void Update()
    {
        float distanceMove = cam.transform.position.x * (1 - parallaxEffect);
        float distanceToMove = cam.transform.position.x * parallaxEffect;

        transform.position = new Vector3(xPosition + distanceToMove,transform.position.y);

        if (distanceMove > xPosition + length)
        {
            xPosition = xPosition + length;
        }else if (distanceMove < xPosition - length)
        {
            xPosition = xPosition - length;
        }
    }
}
