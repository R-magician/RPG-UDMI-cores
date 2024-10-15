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
    //移动方向
    public Vector2 inputDirection;
    
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

    private void Update()
    {
        //获取移动时候的值
        inputDirection = inputControl.Player.Move.ReadValue<Vector2>();
    }

    private void OnEnable()
    {
        //启动控制系统
        inputControl.Enable();
        //玩家冲刺
        inputControl.Player.Dash.started += Dash;
        //水晶
        inputControl.Player.Crystal.started += Crystal;
        //移除物品
        inputControl.Item.Remove.performed += Remove;
    }
    //玩家冲刺
    private void Dash(InputAction.CallbackContext obj)
    {
        PlayerManager.instance.Player.Dash();
    }

    //水晶
    private void Crystal(InputAction.CallbackContext obj)
    {
        PlayerManager.instance.Player.Crystal();
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
