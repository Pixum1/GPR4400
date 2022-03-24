using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

[RequireComponent(typeof(Rigidbody), typeof(NavMeshAgent))]
public class StateMachine : MonoBehaviour
{
    public NPCStats stats;

    [HideInInspector]
    public float moveSpeed;
    [HideInInspector]
    public float moveRadius;
    [HideInInspector]
    public float sightRadius;
    [HideInInspector]
    public float attackDamage;
    [HideInInspector]
    public float attackRange;
    [HideInInspector]
    public Vector3 startPosition;
    [HideInInspector]
    public LayerMask enemyLayer;

    protected BaseState currentState;

    //[HideInInspector]
    public GameObject target;
    [HideInInspector]
    public NavMeshAgent agent;

    public virtual void Awake()
    {
        agent = GetComponent<NavMeshAgent>();
        GetValues();
        startPosition = transform.position;
    }
    public virtual void Start()
    {
        currentState = GetInitialState();

        if (currentState != null)
            currentState.Enter();
    }

    public virtual void Update()
    {
        if (currentState != null)
            currentState.UpdateLogic();
    }

    public virtual void FixedUpdate()
    {
        if(currentState != null)
            currentState.UpdatePhysics();
    }

    public void ChangeState(BaseState _newState)
    {
        Debug.Log(_newState);
        currentState.Exit();

        currentState = _newState;
        currentState.Enter();
    }

    public virtual BaseState GetInitialState()
    {
        return null;
    }

    private void GetValues()
    {
        moveSpeed = stats.moveSpeed;
        moveRadius = stats.moveRadius;
        sightRadius = stats.sightRadius;
        attackDamage = stats.attackDamage;
        attackRange = stats.attackRange;
        enemyLayer = stats.enemyLayer;
    }
}
