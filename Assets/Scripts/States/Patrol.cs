using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Patrol : BaseState
{
    private EntitySM stateMachine;

    private Vector3 destination;

    public Patrol(EntitySM _sm) : base("Patrol", _sm)
    {
        stateMachine = _sm;
    }

    public override void Enter()
    {
        base.Enter();

        destination = Vector3.zero; //<------ Get new patrol point here!
    }
    public override void UpdateLogic()
    {
        base.UpdateLogic();

        //If the patrol point has been reached
        if(Vector3.Distance(stateMachine.position, destination) <= .05f)
        {
            //Return to idle
            stateMachine.ChangeState(stateMachine.idleState);
        }
        else
        {
            //Go to patrol point
            stateMachine.agent.destination = destination;
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
