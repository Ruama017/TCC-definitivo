using UnityEngine;

public class BoglinPatrolState : BoglinBaseState
{
    private int currentPoint = 0;
    private int direction = 1; // 1 = indo para direita, -1 = voltando para esquerda
    private Vector3[] patrolPoints;
    private float waitTime = 0.1f;
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

            if (distanceToPlayer <= boglin.attackRange)
            {
                boglin.SwitchState(boglin.GetAttackState());
                return;
            }

            if (distanceToPlayer <= boglin.detectionRange)
            {
                boglin.SwitchState(boglin.GetWalkState());
                return;
            }
        }

        Vector3 target = patrolPoints[currentPoint];
        boglin.MoveTowards(target);

        if (Vector3.Distance(boglin.transform.position, target) < 0.05f)
        {
            waitTimer += Time.deltaTime;

            if (waitTimer >= waitTime)
            {
                if (currentPoint == 0)
                    direction = 1;
                else if (currentPoint == patrolPoints.Length - 1)
                    direction = -1;

                currentPoint += direction;
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
