//文字弹出

using System;
using TMPro;
using UnityEngine;

public class PopUpTextFx : MonoBehaviour
{
    private TextMeshPro myText;
    
    //弹出速度
    [SerializeField] private float speed;
    [SerializeField] private float desapearanceSpeed;
    
    [SerializeField] private float colorDeasapearanceSpeed;

    //存活时间
    [SerializeField] private float lifeTime;

    private float textTimer;

    private void Start()
    {
        myText = GetComponent<TextMeshPro>();
        textTimer = lifeTime;
    }

    private void Update()
    {
        textTimer -= Time.deltaTime;

        if (textTimer < 0)
        {
            transform.position = Vector2.MoveTowards(transform.position,
                new Vector2(transform.position.x, transform.position.y + 1),desapearanceSpeed * Time.deltaTime);
            float alpha = myText.color.a - colorDeasapearanceSpeed * Time.deltaTime;
            myText.color = new Color(myText.color.r, myText.color.g, myText.color.b, alpha);

            if (myText.color.a < 50)
            {
                speed = desapearanceSpeed;
            }

            if (myText.color.a <= 0)
            {
                Destroy(gameObject);
            }
        }
    }
}
