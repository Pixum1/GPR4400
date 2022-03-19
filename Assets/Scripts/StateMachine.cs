using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StateMachine : MonoBehaviour
{
    public NPCStats stats;
    private BaseState currentState;

    private void Start()
    {
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
}
