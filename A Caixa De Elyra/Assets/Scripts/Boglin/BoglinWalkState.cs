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

        // Se estiver muito longe → volta para patrulhar
        if (distance > boglin.detectionRange)
        {
            boglin.SwitchState(boglin.GetPatrolState());
            return;
        }

        // Se estiver perto o suficiente → ataca
        if (distance <= boglin.attackRange)
        {
            boglin.SwitchState(boglin.GetAttackState());
            return;
        }

        // Caso contrário, persegue o player mantendo o Y do Boglin
        Vector3 targetPos = new Vector3(boglin.player.position.x, boglin.rb.position.y, 0);
        boglin.MoveTowards(targetPos);
    }

    public override void ExitState(BoglinController boglin)
    {
        if (boglin.anim != null)
            boglin.anim.SetBool("IsWalking", false);
        boglin.StopMoving();
    }
}
