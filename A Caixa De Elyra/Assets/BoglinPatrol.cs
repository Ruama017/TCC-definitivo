using UnityEngine;

public class BoglinPatrol : StateMachineBehaviour
{
    private BoglinController boglin;

    private Vector3 leftPoint;
    private Vector3 rightPoint;
    private Vector3 target;

    private bool goingRight = true;

    // Chamado quando entra no estado
    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        boglin = animator.GetComponent<BoglinController>();

        if (boglin == null)
        {
            Debug.LogError("BoglinController não encontrado!");
            return;
        }

        leftPoint = boglin.leftPatrolPoint.position;
        rightPoint = boglin.rightPatrolPoint.position;

        target = rightPoint;

        animator.SetBool("IsWalking", true);
    }

    // Chamado a cada frame enquanto estiver no estado
    override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        if (boglin == null) return;

        // Movimento de patrulha
        boglin.MoveTowards(target);

        // Chegou no ponto?
        if (Vector2.Distance(animator.transform.position, target) < 0.2f)
        {
            goingRight = !goingRight;
            target = goingRight ? rightPoint : leftPoint;
        }

        // Se não tiver player, continua patrulhando
        if (boglin.player == null) return;

        float distanceToPlayer = Vector2.Distance(
            animator.transform.position,
            boglin.player.position
        );

        // Player detectado → sair da patrulha
        if (distanceToPlayer <= boglin.detectionRange)
        {
            animator.SetBool("IsWalking", true);
        }
    }

    // Chamado ao sair do estado
    override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        animator.SetBool("IsWalking", false);
        boglin.StopMoving();
    }
}
