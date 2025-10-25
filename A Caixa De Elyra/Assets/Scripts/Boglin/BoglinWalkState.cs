using UnityEngine;

public class BoglinWalkState : BoglinBaseState
{
    public override void EnterState(BoglinController boglin)
    {
        if (boglin.anim != null)
            boglin.anim.SetBool("isWalking", true);
    }

    public override void UpdateState(BoglinController boglin)
    {
        if (boglin.player == null) return;

        float distance = Vector2.Distance(boglin.transform.position, boglin.player.position);

        if (distance <= boglin.attackRange)
        {
            boglin.SwitchState(boglin.attackState);
            return;
        }

        boglin.MoveTowards(boglin.player.position);
    }

    public override void ExitState(BoglinController boglin)
    {
        boglin.StopMoving();
    }
}
