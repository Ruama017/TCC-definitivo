using UnityEngine;

public class BoglinChaseState : BoglinState
{
    public BoglinChaseState(BoglinController boglin) : base(boglin) { }

    public override void Enter()
    {
        if (boglin.animator != null)
            boglin.animator.SetBool("isWalking", true);
    }

    public override void Exit()
    {
        if (boglin.animator != null)
            boglin.animator.SetBool("isWalking", false);
    }

    public override void LogicUpdate()
    {
        if (boglin.player == null)
            return;

        float distToPlayer = Vector2.Distance(boglin.transform.position, boglin.player.position);

        // Muda para Attack se estiver perto
        if (distToPlayer <= boglin.attackDistance)
        {
            boglin.ChangeState(boglin.attackState);
            return;
        }

        // Muda para Patrol se estiver longe
        if (distToPlayer > boglin.chaseDistance)
        {
            boglin.ChangeState(boglin.patrolState);
            return;
        }

        // Move em direção ao player
        boglin.MoveTowards(boglin.player.position);
    }
}
