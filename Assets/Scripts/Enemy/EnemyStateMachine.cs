//敌人状态机
using UnityEngine;

public class EnemyStateMachine 
{
    //当前状态
    public EnemyState currentState { get; private set; }

    //初始化状态
    public void Initialize(EnemyState startState)
    {
        currentState = startState;
        currentState.Enter();
    }

    public void ChangeState(EnemyState newState)
    {
        currentState.Exit();
        currentState = newState;
        currentState.Enter();
    }
}
