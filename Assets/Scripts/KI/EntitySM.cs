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

        #region Detection
        //-- Set target if enemies in range and has no target
        if (GetEnemies(sightRadius).Length > 0)
            if (target == null)
                target = GetEnemies(sightRadius)[0].transform.gameObject;

        if (GetEnemies(sightRadius).Length == 0)
        {
            if (target != null)
                target = null;
        }
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

    #region StateTransitionConditions
    private void CheckIdleConditions()
    {
        //-- has target
        if (target != null)
        {//-- is in attackRange
            if (InAttackRange())
                ChangeState(attackingState);
            //-- not in attackRange
            else
                ChangeState(followingState);
        }
        //-- no target
        else
            //-- idleTimer < 0
            if (idleState.idleTimer < 0)
            ChangeState(roamingState);
    }
    private void CheckRoamingConditions()
    {
        //-- has target
        if (target != null)
        {//-- is in attackRange
            if (InAttackRange())
                ChangeState(attackingState);
            //-- not in attackRange
            else
                ChangeState(followingState);
        }
        //-- calculated path is not valid 
        if (agent.pathStatus == UnityEngine.AI.NavMeshPathStatus.PathPartial)
            ChangeState(idleState);

        //-- destination reached
        if (agent.remainingDistance == 0 && agent.path != null)
            ChangeState(idleState);
    }
    private void CheckFollowingConditions()
    {
        //-- Has target
        if (target != null)
        {//-- Is in attackRange
            if (InAttackRange())
                ChangeState(attackingState);
        }
        //-- no target
        else
            ChangeState(idleState);
    }
    private void CheckAttackingConditions()
    {
        //-- Has target
        if (target != null)
        {//-- not in attackRange
            if (!InAttackRange())
                ChangeState(followingState);
        }
        //-- no target
        else
            ChangeState(idleState);
    }
    #endregion

    #region RuntimeCalculations
    public RaycastHit[] GetEnemies(float _radius)
    {
        return Physics.SphereCastAll(transform.position, _radius, Vector3.up, float.MaxValue, enemyLayer);
    }
    private bool InAttackRange()
    {
        return Vector3.Distance(transform.position, target.transform.position) < attackRange;
    }
    #endregion

    public void Attack()
    {
        GameObject go = Instantiate(bulletPrefab, transform.position, Quaternion.identity, transform);

        Bullet b = go.GetComponent<Bullet>();
        b.attackDamage = attackDamage;
        b.mask = enemyLayer;

        go.GetComponent<Rigidbody>().AddForce(transform.forward * 10, ForceMode.Impulse);
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
            Gizmos.DrawWireSphere(transform.position, stats.SightRadius);

        //-- Move Radius
        Gizmos.color = Color.gray;
        Gizmos.DrawWireSphere(startPosition, stats.MoveRadius);

        //-- Attack Radius
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, stats.AttackRange);
    }
}
