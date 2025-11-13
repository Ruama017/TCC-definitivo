using UnityEngine;

public class BoglinPatrolState : BoglinBaseState
{
    private int currentPoint = 0;
    private Vector3[] patrolPoints;
    private float waitTime = 2f;
    private float waitTimer;

    public BoglinPatrolState(Vector3 leftPoint, Vector3 rightPoint)
    {
        patrolPoints = new Vector3[] { leftPoint, rightPoint };
    }

    public override void EnterState(BoglinController boglin)
    {
        if (boglin.anim != null)
            boglin.anim.SetBool("IsWalking", true);

        waitTimer = 0;
    }

    public override void UpdateState(BoglinController boglin)
    {
        if (boglin.player == null) return;

        float distanceToPlayer = Vector2.Distance(boglin.transform.position, boglin.player.position);

        // Se o player estiver dentro do alcance de ataque
        if (distanceToPlayer <= boglin.attackRange)
        {
            boglin.SwitchState(boglin.GetAttackState());
            return;
        }

        // Se estiver dentro do alcance de perseguição, muda pro WalkState
        if (distanceToPlayer <= boglin.detectionRange)
        {
            boglin.SwitchState(boglin.GetWalkState());
            return;
        }

        // Patrulha normal entre pontos
        Vector3 target = patrolPoints[currentPoint];
        boglin.MoveTowards(target);

        if (Vector2.Distance(boglin.transform.position, target) < 0.1f)
        {
            waitTimer += Time.deltaTime;

            if (waitTimer >= waitTime)
            {
                currentPoint = (currentPoint + 1) % patrolPoints.Length;
                waitTimer = 0;
            }
        }
    }

    public override void ExitState(BoglinController boglin)
    {
        if (boglin.anim != null)
            boglin.anim.SetBool("IsWalking", false);
    }
}
