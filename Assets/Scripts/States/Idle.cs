using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Idle : BaseState
{
    private EntitySM stateMachine;

    public Idle(EntitySM _sm) : base(_sm) 
    { 
        stateMachine = _sm;
    }


    public override void Enter()
    {
        base.Enter();

        stateMachine.target = null; //-> Reset target
        stateMachine.stats.runTimeSight = stateMachine.stats.sightRadius; //-> Reset runtimeSight
        stateMachine.stats.runTimeMoveSpeed = stateMachine.stats.moveSpeed; //-> Reset agent movespeed to normal value
    }

    public override void UpdateLogic()
    {
        base.UpdateLogic();

        //-- reached initial position when coming from following state
        if (stateMachine.agent.remainingDistance <= 0)
        {
            //-- Has target
            if (stateMachine.target != null)
                stateMachine.ChangeState(stateMachine.followingState);

            //-- Has no target
            else
                stateMachine.ChangeState(stateMachine.roamingState);
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
