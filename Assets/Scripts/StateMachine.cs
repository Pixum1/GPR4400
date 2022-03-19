using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

[RequireComponent(typeof(Rigidbody), typeof(NavMeshAgent))]
public class StateMachine : MonoBehaviour
{
    public NPCStats stats;
    private BaseState currentState;

    [HideInInspector]
    public Vector3 position;
    [HideInInspector]
    public GameObject target;
    [HideInInspector]
    public NavMeshAgent agent;

    private void Awake()
    {
        agent = GetComponent<NavMeshAgent>();
    }
    private void Start()
    {
        currentState = GetInitialState();

        if (currentState != null)
            currentState.Enter();
    }

    private void Update()
    {
        if(currentState != null)
            currentState.UpdateLogic();
    }

    private void FixedUpdate()
    {
        if(currentState != null)
            currentState.UpdatePhysics();
    }

    public void ChangeState(BaseState _newState)
    {
        currentState.Exit();

        currentState = _newState;
        currentState.Enter();
    }

    public virtual BaseState GetInitialState()
    {
        return null;
    }
}
