using UnityEngine;

public class BoglinPatrolState : BoglinBaseState
{
    private int currentPoint = 0;
    private Vector3[] patrolPoints;
    private float waitTime = 0f;
    private float waitTimer;

    public BoglinPatrolState(Vector3 leftPoint, Vector3 rightPoint, float fixedY)
    {
        patrolPoints = new Vector3[]
        {
            new Vector3(leftPoint.x, fixedY, 0),
            new Vector3(rightPoint.x, fixedY, 0)
        };
    }

    public override void EnterState(BoglinController boglin)
    {
        if (boglin.anim != null)
            boglin.anim.SetBool("IsWalking", true);

        waitTimer = 0;
    }

    public override void UpdateState(BoglinController boglin)
    {
        if (boglin.player != null)
        {
            float distanceToPlayer = Vector2.Distance(boglin.transform.position, boglin.player.position);

            // Se o player estiver dentro do alcance de ataque
            if (distanceToPlayer <= boglin.attackRange)
            {
                boglin.SwitchState(boglin.GetAttackState());
                return;
            }

            // Se estiver dentro do alcance de perseguição, muda para WalkState
            if (distanceToPlayer <= boglin.detectionRange)
            {
                boglin.SwitchState(boglin.GetWalkState());
                return;
            }
        }

        // Patrulha padrão
        Vector3 target = patrolPoints[currentPoint];
        boglin.MoveTowards(target);

        if (Mathf.Abs(boglin.transform.position.x - target.x) < 0.1f)
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
