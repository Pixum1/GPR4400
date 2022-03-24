using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Attacking : BaseState
{
    private EntitySM stateMachine;

    public Attacking(EntitySM _sm) : base(_sm)
    {
        stateMachine = _sm;
    }

    public override void Enter()
    {
        base.Enter();
        stateMachine.agent.ResetPath();
    }
    public override void UpdateLogic()
    {
        base.UpdateLogic();

        
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
