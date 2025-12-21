using UnityEngine;

public class BoglinWalk : StateMachineBehaviour
{
    BoglinController boglin;
    float lastAttackTime;

    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        boglin = animator.GetComponent<BoglinController>();
        lastAttackTime = 0f;
    }

    override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        if (boglin.player == null) return;

        float distance = Vector2.Distance(
            animator.transform.position,
            boglin.player.position
        );

        // 🔴 DISPARA ATAQUE (TRIGGER)
        if (distance <= boglin.attackRange)
        {
            animator.SetTrigger("Attack");
            return;
        }

        // Volta para patrulha
        if (distance > boglin.detectionRange)
        {
            animator.SetBool("isWalking", false);
            return;
        }

        // Persegue
        Vector3 target = new Vector3(
            boglin.player.position.x,
            boglin.rb.position.y,
            0
        );

        boglin.MoveTowards(target);
    }

    override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        boglin.StopMoving();
    }
}
