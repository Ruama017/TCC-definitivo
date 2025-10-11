using UnityEngine;

public class BoglinAttackState : BoglinState
{
    private float attackTimer = 0f;
    private float attackCooldown = 1f; // tempo entre ataques

    public BoglinAttackState(BoglinController boglin) : base(boglin) { }

    public override void Enter()
    {
        if (boglin.animator != null)
            boglin.animator.SetTrigger("Attack");
        attackTimer = 0f;
    }

    public override void Exit()
    {
        if (boglin.animator != null)
            boglin.animator.ResetTrigger("Attack");
    }

    public override void LogicUpdate()
    {
        if (boglin.player == null)
            return;

        float distToPlayer = Vector2.Distance(boglin.transform.position, boglin.player.position);

        // Se player saiu da distância de ataque, volta a Chase
        if (distToPlayer > boglin.attackDistance)
        {
            boglin.ChangeState(boglin.chaseState);
            return;
        }

        // Aplica ataque periodicamente
        attackTimer += Time.deltaTime;
        if (attackTimer >= attackCooldown)
        {
            boglin.AttackPlayer();
            attackTimer = 0f;
        }

        // Fica de frente para o player
        Vector3 scale = boglin.transform.localScale;
        scale.x = (boglin.player.position.x - boglin.transform.position.x > 0 ? 1 : -1) * Mathf.Abs(scale.x);
        boglin.transform.localScale = scale;
    }
}