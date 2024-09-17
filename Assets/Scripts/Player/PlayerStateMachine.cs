//玩家状态机
using UnityEngine;

public class PlayerStateMachine 
{
    //当前动画状态--可读不可写
    public PlayerState currentState{ get; private set; }

    //初始化状态
    public void initialize(PlayerState initialState)
    {
        //将当前状态等于开始状态
        currentState = initialState;
        currentState.Enter();
    }
    
    //改变状态
    public void ChangeState(PlayerState newState)
    {
        currentState.Exit();
        currentState = newState;
        currentState.Enter();
    }
}
