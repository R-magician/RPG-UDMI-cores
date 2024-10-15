//输入系统

using System;
using UnityEngine;
using UnityEngine.InputSystem;

[DefaultExecutionOrder(-100)]
public class InputManager : MonoBehaviour
{
    //单例模式
    public static InputManager instance;
    
    //玩家控制系统
    public PlayerInputControl inputControl;
    
    private void Awake()
    {
        if (instance == null)
            instance = this;
        else
        {
            //实例已经存在，要将当前创建的实例给销毁掉
            Destroy(gameObject);
        }
        
        //创建一个实例
        inputControl = new PlayerInputControl();
    }
    private void OnEnable()
    {
        //启动控制系统
        inputControl.Enable();
        //移除物品
        inputControl.Item.Remove.performed += Remove;
    }

    private void OnDisable()
    {
        //关闭控制系统
        inputControl.Disable();
        //移除物品
        inputControl.Item.Remove.performed -= Remove;
    }
    
    //移除物品
    private void Remove(InputAction.CallbackContext obj)
    {
    }
    
}
