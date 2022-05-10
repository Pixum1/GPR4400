using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BaseState
{
    protected StateMachine sm;

    public BaseState(StateMachine _sm)
    {
        sm = _sm;
    }

    public virtual void Enter() { }
    public virtual void UpdateLogic() { }
    public virtual void Exit() { }
}
