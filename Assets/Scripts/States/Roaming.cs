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
        stateMachine.agent.destination = new Vector3(stateMachine.stats.startPosition.x + Random.insideUnitCircle.x, 0, stateMachine.stats.startPosition.z + Random.insideUnitCircle.y) * stateMachine.stats.moveRadius;
    }
    public override void UpdateLogic()
    {
        base.UpdateLogic();

        //-- If has target
        if(stateMachine.target != null)
            stateMachine.ChangeState(stateMachine.followingState);

        //-- If calculated path is not valid
        if(stateMachine.agent.pathStatus == UnityEngine.AI.NavMeshPathStatus.PathPartial)
            stateMachine.ChangeState(stateMachine.idleState);

        //-- If destination was reached
        if (stateMachine.agent.remainingDistance == 0 && stateMachine.agent.path != null)
            stateMachine.ChangeState(stateMachine.idleState);
    }
    public override void UpdatePhysics()
    {
        base.UpdatePhysics();
        
    }
    public override void Exit()
    {
        base.Exit();
        stateMachine.agent.ResetPath();
    }
}
