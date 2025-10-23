using UnityEngine;

public abstract class BoglinBaseState{
    public abstract void EnterState(BoglinController boglin);
    public abstract void UpdateState(BoglinController boglin);
    public abstract void ExitState(BoglinController boglin);
}
