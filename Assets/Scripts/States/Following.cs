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
        stateMachine.runTimeSight = stateMachine.stats.sightRadius * 2f;
        stateMachine.runTimeMoveSpeed = stateMachine.stats.moveSpeed * 2f;
    }
    public override void UpdateLogic()
    {
        base.UpdateLogic();

        //-- Has target
        if (stateMachine.target != null)
        {
            stateMachine.agent.destination = stateMachine.target.transform.position;

            //-- Is in attackRange
            if (Vector3.Distance(stateMachine.transform.position, stateMachine.target.transform.position) <= stateMachine.stats.attackRange)
                stateMachine.ChangeState(stateMachine.attackingState);
        }
        //-- Has no target
        else
        {
            stateMachine.agent.destination = initialPosition; //-> Return to initial position and go to idle state
            stateMachine.target = null; //-> Reset target
            stateMachine.ChangeState(stateMachine.idleState);
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
