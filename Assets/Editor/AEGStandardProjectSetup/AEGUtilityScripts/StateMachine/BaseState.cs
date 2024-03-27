using System;

public abstract class BaseState<EState> where EState : Enum
{
    public BaseState(EState key)
    {
        StateKey = key;
    }
    
    public EState StateKey { get; private set; }
    public abstract void UpdateState();
    public abstract void EnterState(EState lastState);
    public abstract void ExitState();
}
