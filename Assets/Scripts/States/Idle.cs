using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Idle : BaseState
{
    private EntitySM stateMachine;
    public float idleTimer;
    private float idleTime = Random.Range(1,4);

    public Idle(EntitySM _sm) : base(_sm) 
    { 
        stateMachine = _sm;
    }


    public override void Enter()
    {
        base.Enter();

        idleTimer = idleTime;
        stateMachine.target = null; //-> Reset target
        stateMachine.sightRadius = stateMachine.stats.sightRadius; //-> Reset runtimeSight
        stateMachine.moveSpeed = stateMachine.stats.moveSpeed; //-> Reset agent movespeed to normal value
    }

    public override void UpdateLogic()
    {
        base.UpdateLogic();

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
