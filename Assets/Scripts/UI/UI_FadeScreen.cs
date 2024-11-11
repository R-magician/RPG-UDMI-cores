//画面淡入

using System;
using UnityEngine;

public class UI_FadeScreen : MonoBehaviour
{
    private Animator anim;

    private void Start()
    {
        anim = GetComponent<Animator>();
    }

    //淡出
    public void FadeOut()
    {
        anim.SetTrigger("fadrOut");
    }
    
    //淡入
    public void FadeIn()
    {
        anim.SetTrigger("fadrIn");
    }
}
