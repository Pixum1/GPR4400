using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Roaming : BaseState
{
    private EntitySM stateMachine;

    public Roaming(EntitySM _sm) : base(_sm)
    {
        stateMachine = _sm;
    }

    public override void Enter()
    {
        base.Enter();

        //-- Set path to random destination
        Vector3 circle = Random.insideUnitSphere * stateMachine.stats.MoveRadius;
        stateMachine.agent.SetDestination(stateMachine.startPosition + circle);
    }
    public override void UpdateLogic()
    {
        base.UpdateLogic();
    }
    public override void Exit()
    {
        base.Exit();
        stateMachine.agent.ResetPath();
    }
}
