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

    
    [HideInInspector]
    public Vector3 startPosition;
    [HideInInspector]
    public float runTimeSight;
    [HideInInspector]
    public float runTimeMoveSpeed;

    public override void Awake()
    {
        base.Awake();

        runTimeSight = stats.sightRadius;
        runTimeMoveSpeed = stats.moveSpeed;

        idleState = new Idle(this);
        roamingState = new Roaming(this);
        attackingState = new Attacking(this);
        followingState = new Following(this);
    }

    public override void Update()
    {
        base.Update();

        agent.speed = runTimeMoveSpeed;

        //-- Set target if enemies in range and has no target
        if (GetEnemies(runTimeSight).Length > 0)
            target = GetEnemies(runTimeSight)[0].transform.gameObject;
        else if (GetEnemies(runTimeSight).Length == 0)
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

    private void OnDrawGizmos()
    {
        //-- Sight Radius
        Gizmos.color = Color.yellow;
        if (runTimeSight > 0)
            Gizmos.DrawWireSphere(transform.position, runTimeSight);
        else
            Gizmos.DrawWireSphere(transform.position, stats.sightRadius);

        //-- Move Radius
        Gizmos.color = Color.gray;
        Gizmos.DrawWireSphere(transform.position, stats.moveRadius);

        //-- Attack Radius
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, stats.attackRange);
    }
}
