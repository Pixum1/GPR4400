using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

[RequireComponent(typeof(Rigidbody), typeof(NavMeshAgent))]
public class StateMachine : MonoBehaviour
{
    public NPCStats stats;
    protected BaseState currentState;

    [HideInInspector]
    public GameObject target;
    [HideInInspector]
    public NavMeshAgent agent;

    public virtual void Awake()
    {
        agent = GetComponent<NavMeshAgent>();
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
}
