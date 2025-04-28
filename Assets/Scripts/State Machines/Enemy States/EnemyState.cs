using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyState
{
    protected EnemyBehaviour enemy;
    protected EnemyStateMachine enemyStateMachine;
    protected PlayerMovement player;
    protected Rigidbody enemyRb;

    public EnemyState(EnemyBehaviour enemy, EnemyStateMachine enemyStateMachine)
    {
        this.enemy = enemy;
        this.enemyStateMachine = enemyStateMachine;
    }

    public virtual void EnterState() 
    { 
        player = Object.FindObjectOfType<PlayerMovement>();
        enemyRb = enemy.GetComponent<Rigidbody>();
    }
    public virtual void ExitState() { }
    public virtual void FrameUpdate() { }
    public virtual void PhysicsUpdate() { }
}
