using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyStateMachine
{
    public EnemyState CurrentEnemyState { get; set; }

    public void Initialize(EnemyState state)
    {
        CurrentEnemyState = state;
        CurrentEnemyState.EnterState();
    }
    public void ChangeState(EnemyState state)
    {
        CurrentEnemyState.ExitState();
        CurrentEnemyState = state;
        CurrentEnemyState.EnterState();
    }
}
