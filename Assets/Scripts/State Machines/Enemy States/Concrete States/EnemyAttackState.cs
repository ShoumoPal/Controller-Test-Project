using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyAttackState : EnemyState
{
    private float elapsedTime;
    private float currentShotTime;

    public EnemyAttackState(EnemyBehaviour enemy, EnemyStateMachine enemyStateMachine) : base(enemy, enemyStateMachine)
    {
    }

    public override void EnterState()
    {
        base.EnterState();

        currentShotTime = Random.Range(2f, 7f);
    }

    public override void ExitState()
    {
        base.ExitState();
    }

    public override void FrameUpdate()
    {
        base.FrameUpdate();

        if (player != null)
        {
            #region Shoot
            if (elapsedTime >= currentShotTime)
            {
                enemy.ShootBullet();
                elapsedTime = 0;
                currentShotTime = Random.Range(1f, 5f);
            }
            else
                elapsedTime += Time.deltaTime;
            #endregion 
        }
    }

    public override void PhysicsUpdate()
    {
        base.PhysicsUpdate();

        if (player != null)
        {
            #region Chase
            var floorTramsform = new Vector3(player.transform.position.x + 0.5f,
                                       enemy.transform.position.y,
                                       player.transform.position.z);

            enemy.transform.LookAt(floorTramsform);
            var direction = (floorTramsform - enemy.transform.position).normalized;
            enemyRb.MovePosition(enemy.transform.position + (enemy.moveSpeed * Time.deltaTime * direction));
            #endregion 
        }
    }
}
