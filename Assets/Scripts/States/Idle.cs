using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Idle : BaseState
{
    private EntitySM stateMachine;

    public Idle(EntitySM _sm) : base("Idle", _sm) 
    { 
        stateMachine = _sm;
    }


    public override void Enter()
    {
        base.Enter();
    }

    public override void UpdateLogic()
    {
        base.UpdateLogic();

        //If enemy is in sight
        if (Physics.SphereCast(stateMachine.stats.startPosition, stateMachine.stats.sightRadius, Vector3.up, out RaycastHit hitInfo))
        {
            if (hitInfo.transform.gameObject.layer == stateMachine.stats.enemies)
            {
                //Follow the enemy
                stateMachine.ChangeState(stateMachine.followState);
            }
        }
        else
        {
            //Return to patroling
            stateMachine.ChangeState(stateMachine.patrolState);
        }
    }

    public override void UpdatePhysics()
    {
        base.UpdatePhysics();
    }

    public override void Exit()
    {
        base.Exit();
    }
}
