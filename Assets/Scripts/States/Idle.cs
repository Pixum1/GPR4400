using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Idle : BaseState
{
    private EntitySM stateMachine;
    public float idleTimer;

    public Idle(EntitySM _sm) : base(_sm) 
    { 
        stateMachine = _sm;
    }


    public override void Enter()
    {
        base.Enter();

        idleTimer = Random.Range(1, 4);
        stateMachine.sightRadius = stateMachine.stats.sightRadius; //-> Reset runtimeSight
        stateMachine.moveSpeed = stateMachine.stats.moveSpeed; //-> Reset agent movespeed to normal value
    }

    public override void UpdateLogic()
    {
        base.UpdateLogic();

        if(stateMachine.agent.remainingDistance == 0)
            idleTimer -= Time.deltaTime;
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
