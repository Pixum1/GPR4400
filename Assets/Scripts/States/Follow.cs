using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Follow : BaseState
{
    private EntitySM stateMachine;

    public Follow(EntitySM _sm) : base("Follow", _sm)
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
        //if has no target
        if(stateMachine.target == null)
        {
            //Return to idle state
            stateMachine.ChangeState(stateMachine.idleState);
        }

        //If target is in range
        if (Vector3.Distance(stateMachine.position, stateMachine.target.transform.position) <= stateMachine.stats.attackRange)
        {
            //Start to attack target
            stateMachine.ChangeState(stateMachine.attackState);
        }
        //If has target but is not in range
        else if(stateMachine.target != null)
        {
            //Go to target position
            stateMachine.agent.destination = stateMachine.target.transform.position;
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
