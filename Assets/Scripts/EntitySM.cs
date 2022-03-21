using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EntitySM : StateMachine
{
    [HideInInspector]
    public Idle idleState;
    [HideInInspector]
    public Roaming roamingState;
    [HideInInspector]
    public Following followingState;
    [HideInInspector]
    public Attacking attackingState;

    public override void Awake()
    {
        base.Awake();

        stats.runTimeSight = stats.sightRadius;
        stats.runTimeMoveSpeed = stats.moveSpeed;

        idleState = new Idle(this);
        roamingState = new Roaming(this);
        attackingState = new Attacking(this);
        followingState = new Following(this);
    }

    public override void Update()
    {
        base.Update();

        agent.speed = stats.runTimeMoveSpeed;

        //-- Set target if enemies in range and has no target
        if (GetEnemies(stats.runTimeSight).Length > 0)
            target = GetEnemies(stats.runTimeSight)[0].transform.gameObject;
        else if (GetEnemies(stats.runTimeSight).Length == 0)
            target = null;
    }

    public RaycastHit[] GetEnemies(float _radius)
    {
        return Physics.SphereCastAll(transform.position, _radius, Vector3.up, Mathf.Infinity, stats.enemyLayer);
    }

    public override BaseState GetInitialState()
    {
        return idleState;
    }
}
