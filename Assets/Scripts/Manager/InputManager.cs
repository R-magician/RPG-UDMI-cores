//输入系统

using System;
using UnityEngine;
using UnityEngine.InputSystem;

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

    private void Start()
    {
        inputControl.Item.Remove.started += Remove;
        Debug.Log(inputControl.Item.Remove);
    }

    private void Remove(InputAction.CallbackContext obj)
    {
        Debug.Log("12323");
    }
}
