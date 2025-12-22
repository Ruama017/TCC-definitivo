using UnityEngine;

public class BoglinPatrol : StateMachineBehaviour
{
    private BoglinController boglin;

    private Vector3 leftPoint;
    private Vector3 rightPoint;
    private Vector3 currentTarget;

    private const float reachDistance = 0.25f;

    public override void OnStateEnter(
        Animator animator,
        AnimatorStateInfo stateInfo,
        int layerIndex)
    {
        Debug.Log(">>> ENTROU NO STATE: PATROL");

        boglin = animator.GetComponent<BoglinController>();

        if (boglin == null)
        {
            Debug.LogError("BoglinController NÃO encontrado");
            return;
        }

        leftPoint = boglin.leftPatrolPoint.position;
        rightPoint = boglin.rightPatrolPoint.position;

        currentTarget = rightPoint;

        animator.SetBool("isWalkin", true);
    }

    public override void OnStateUpdate(
        Animator animator,
        AnimatorStateInfo stateInfo,
        int layerIndex)
    {
        if (boglin == null) return;

        Debug.Log("PATROL UPDATE");

        // Movimento
        float distToTarget =
            Vector2.Distance(animator.transform.position, currentTarget);

        if (distToTarget > reachDistance)
        {
            boglin.MoveTowards(currentTarget);
        }
        else
        {
            // troca de ponto
            currentTarget =
                currentTarget == rightPoint ? leftPoint : rightPoint;
        }

        // Detecta player
        if (boglin.player == null) return;

        float distToPlayer = Vector2.Distance(
            animator.transform.position,
            boglin.player.position);

        if (distToPlayer <= boglin.attackRange)
        {
            Debug.Log(">>> TRIGGER ATTACK DISPARADO");
            animator.SetTrigger("Attack");
        }
    }

    public override void OnStateExit(
        Animator animator,
        AnimatorStateInfo stateInfo,
        int layerIndex)
    {
        Debug.Log("<<< SAIU DO STATE: PATROL");

        animator.SetBool("isWalkin", false);

        if (boglin != null)
            boglin.StopMoving();
    }
}
