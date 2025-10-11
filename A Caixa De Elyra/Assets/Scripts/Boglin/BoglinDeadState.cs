using UnityEngine;

public class BoglinDeadState : BoglinState
{
    public BoglinDeadState(BoglinController boglin) : base(boglin) { }

    public override void Enter()
    {
        boglin.animator.SetTrigger("Dead");
        boglin.enabled = false; // para movimentação
    }

    public override void LogicUpdate() { }
    public override void Exit() { }
}
