using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BaseState
{
    public string name;
    protected StateMachine sm;

    public BaseState(string _name, StateMachine _sm)
    {
        name = _name;
        sm = _sm;
    }

    public virtual void Enter() { }
    public virtual void UpdateLogic() { }
    public virtual void UpdatePhysics() { }
    public virtual void Exit() { }
}
