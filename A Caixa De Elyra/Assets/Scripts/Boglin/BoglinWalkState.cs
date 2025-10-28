using UnityEngine;

public class BoglinWalkState : BoglinBaseState
{
    public override void EnterState(BoglinController boglin)
    {
        if (boglin.anim != null)
            boglin.anim.SetBool("IsWalking", true);
    }

    public override void UpdateState(BoglinController boglin)
    {
        if (boglin.player == null) return;

        float distance = Vector2.Distance(boglin.transform.position, boglin.player.position);

        // Troca para AttackState se estiver no alcance
        if (distance <= boglin.attackRange)
        {
            boglin.SwitchState(boglin.GetAttackState());
            return;
        }

        boglin.MoveTowards(boglin.player.position);
    }

    public override void ExitState(BoglinController boglin)
    {
        boglin.StopMoving();
    }
}
