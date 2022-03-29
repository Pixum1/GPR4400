using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Attacking : BaseState
{
    private EntitySM stateMachine;
    public float attackSpeed;
    public float attackTime;

    public Attacking(EntitySM _sm) : base(_sm)
    {
        stateMachine = _sm;
    }

    public override void Enter()
    {
        base.Enter();
        stateMachine.agent.ResetPath();
        attackSpeed = stateMachine.stats.AttackSpeed;
        attackTime = attackSpeed;
    }
    public override void UpdateLogic()
    {
        base.UpdateLogic();

        if(stateMachine.target != null)
        {
            stateMachine.transform.LookAt(stateMachine.target.transform.position);

            attackTime -= Time.deltaTime;

            if (attackTime < 0)
            {
                stateMachine.Attack();
                attackTime = attackSpeed;
            }
        }
    }
    public override void Exit()
    {
        base.Exit();
    }
}
