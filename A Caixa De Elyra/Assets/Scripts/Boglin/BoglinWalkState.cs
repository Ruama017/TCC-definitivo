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
        Debug.Log($"Distância do player: {distance:F2}");

        // Se estiver muito longe → volta a patrulhar
        if (distance > boglin.detectionRange)
        {
            Debug.Log("🔄 Voltando para patrulhar...");
            boglin.SwitchState(boglin.GetPatrolState());
            return;
        }

        // Se estiver perto o suficiente → ataca
        if (distance <= boglin.attackRange)
        {
            Debug.Log("⚔️ Trocando para AttackState!");
            boglin.SwitchState(boglin.GetAttackState());
            return;
        }

        // Caso contrário, persegue o player
        boglin.MoveTowards(boglin.player.position);
    }

    public override void ExitState(BoglinController boglin)
    {
        if (boglin.anim != null)
            boglin.anim.SetBool("IsWalking", false);
        boglin.StopMoving();
    }
}
