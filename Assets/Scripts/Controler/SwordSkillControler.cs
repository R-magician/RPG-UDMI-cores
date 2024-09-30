//飞刀技能控制

using System;
using UnityEngine;

public class SwordSkillControler : MonoBehaviour
{
    //动画控制器
    private Animator anim;
    private Rigidbody2D rb;
    private CircleCollider2D cd;
    private Player player;

    private void Awake()
    {
        anim = GetComponentInChildren<Animator>();
        rb = GetComponent<Rigidbody2D>();
        cd = GetComponent<CircleCollider2D>();
    }

    //开始飞剑--方向，重力
    public void SetupSword(Vector2 dir,float gravityScale)
    {
        rb.linearVelocity = dir;
        rb.gravityScale = gravityScale;
    }

    private void Update()
    {
        transform.right = rb.linearVelocity;
    }
}
