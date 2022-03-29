using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Following : BaseState
{
    private EntitySM stateMachine;

    private Vector3 initialPosition;

    public Following(EntitySM _sm) : base(_sm)
    {
        stateMachine = _sm;
    }

    public override void Enter()
    {
        base.Enter();

        //-- Get initial position before following
        initialPosition = stateMachine.transform.position;

        //-- Adjust Runtime values
        stateMachine.sightRadius = stateMachine.stats.SightRadius * 2f;
        stateMachine.moveSpeed = stateMachine.stats.MoveSpeed * 2f;

    }
    public override void UpdateLogic()
    {
        base.UpdateLogic();

        //-- set destination
        if(stateMachine.target != null)
            stateMachine.agent.SetDestination(stateMachine.target.transform.position);
    }
    public override void Exit()
    {
        base.Exit();

        stateMachine.agent.SetDestination(initialPosition); //-> Return to initial position and go to idle state
    }
}
