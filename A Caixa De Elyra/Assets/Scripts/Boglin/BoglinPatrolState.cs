using UnityEngine;

public class BoglinPatrolState : BoglinState
{
    private int currentPoint = 0;

    public BoglinPatrolState(BoglinController boglin) : base(boglin) { }

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
        if (boglin.player == null || boglin.patrolPoints.Length == 0)
            return;

        // Muda para Chase se player estiver próximo
        float distToPlayer = Vector2.Distance(boglin.transform.position, boglin.player.position);
        if (distToPlayer <= boglin.chaseDistance)
        {
            boglin.ChangeState(boglin.chaseState);
            return;
        }

        // Patrulha entre os pontos
        Vector2 target = boglin.patrolPoints[currentPoint].position;
        boglin.MoveTowards(target);

        if (Vector2.Distance(boglin.transform.position, target) < 0.1f)
            currentPoint = (currentPoint + 1) % boglin.patrolPoints.Length;
    }
}