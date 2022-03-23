using System;
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

        idleState = new Idle(this);
        roamingState = new Roaming(this);
        attackingState = new Attacking(this);
        followingState = new Following(this);
    }

    public override void Start()
    {
        base.Start();
    }

    public override void Update()
    {
        base.Update();

        agent.speed = moveSpeed;

        #region Enemy detection
        //-- Set target if enemies in range and has no target
        if (GetEnemies(sightRadius).Length > 0)
            target = GetEnemies(sightRadius)[0].transform.gameObject;

        else if (GetEnemies(sightRadius).Length == 0)
            target = null;
        #endregion

        if (currentState is Idle)
            CheckIdleConditions();

        else if (currentState is Roaming)
            CheckRoamingConditions();

        else if (currentState is Following)
            CheckFollowingConditions();

        else if (currentState is Attacking)
            CheckAttackingConditions();

    }

    private void CheckIdleConditions()
    {
        //-- has target
        if (target != null)
            //-- is in attackRange
            if (Vector3.Distance(transform.position, target.transform.position) < attackRange)
                ChangeState(attackingState);
            //-- not in attackRange
            else
                ChangeState(followingState);

        //-- no target
        else
            //-- idleTimer < 0
            if(idleState.idleTimer < 0)
                ChangeState(roamingState);
    }
    private void CheckRoamingConditions()
    {
        //-- destination reached

        //-- has target
            //-- not in attackRange
            //-- is in attackRange



        //-- If has target
        if (target != null)
            ChangeState(followingState);

        //-- If calculated path is not valid
        if (agent.pathStatus == UnityEngine.AI.NavMeshPathStatus.PathPartial)
            ChangeState(idleState);

        //-- If destination was reached
        if (agent.remainingDistance == 0 && agent.path != null)
            ChangeState(idleState);
    }

    private void CheckFollowingConditions()
    {
        //-- Has target
        if (target != null)
        {
            agent.destination = target.transform.position;

            //-- Is in attackRange
            if (Vector3.Distance(transform.position, target.transform.position) <= stats.attackRange)
                ChangeState(attackingState);
        }
        //-- Has no target
        else
        {
            ChangeState(idleState);
        }
    }

    private void CheckAttackingConditions()
    {
        //-- Has target
        if (target != null)
        {
            //-- Target is not in range
            if(Vector3.Distance(transform.position, target.transform.position) > stats.attackRange)
                ChangeState(followingState);
        }
        else
            ChangeState(idleState);
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
        if (sightRadius > 0)
            Gizmos.DrawWireSphere(transform.position, sightRadius);
        else
            Gizmos.DrawWireSphere(transform.position, stats.sightRadius);

        //-- Move Radius
        Gizmos.color = Color.gray;
        Gizmos.DrawWireSphere(startPosition, stats.moveRadius);

        //-- Attack Radius
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, stats.attackRange);
    }
}
