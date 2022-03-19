using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EntitySM : StateMachine
{
    [HideInInspector]
    public Idle idleState;
    [HideInInspector]
    public Patrol patrolState;
    [HideInInspector]
    public Follow followState;
    [HideInInspector]
    public Attack attackState;


    private void Awake()
    {
        idleState = new Idle(this);
        patrolState = new Patrol(this);
        attackState = new Attack(this);
        followState = new Follow(this);
    }

    public override BaseState GetInitialState()
    {
        return idleState;
    }
}
