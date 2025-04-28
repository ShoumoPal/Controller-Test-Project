using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyChaseState : EnemyState
{
    public EnemyChaseState(EnemyBehaviour enemy, EnemyStateMachine enemyStateMachine) : base(enemy, enemyStateMachine)
    {
    }

    public override void EnterState()
    {
        base.EnterState();
    }

    public override void ExitState()
    {
        base.ExitState();
    }

    public override void FrameUpdate()
    {
        base.FrameUpdate();
    }

    public override void PhysicsUpdate()
    {
        base.PhysicsUpdate();

        if (player != null)
        {
            var floorTramsform = new Vector3(player.transform.position.x,
                                       enemy.transform.position.y,
                                       player.transform.position.z);

            enemy.transform.LookAt(floorTramsform);
            var direction = (floorTramsform - enemy.transform.position).normalized;
            enemyRb.MovePosition(enemy.transform.position + (enemy.moveSpeed * Time.deltaTime * direction));
        }
    }
}
