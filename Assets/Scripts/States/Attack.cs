using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Attack : BaseState
{
    private EntitySM stateMachine;

    public Attack(EntitySM _sm) : base("Attack", _sm)
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

        //If is in range
        if (Vector3.Distance(stateMachine.position, stateMachine.target.transform.position) <= stateMachine.stats.attackRange)
        {
            ///
            /// Do attack stuff
            ///
        }
        else
        {
            // follow the target
            stateMachine.ChangeState(stateMachine.followState);
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
