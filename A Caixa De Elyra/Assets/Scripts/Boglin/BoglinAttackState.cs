using UnityEngine;

public class BoglinAttackState : BoglinBaseState
{
    public float attackCooldown = 1.5f;
    private float lastAttackTime;

    public override void EnterState(BoglinController boglin)
    {
        if (boglin.anim != null)
            boglin.anim.SetTrigger("Attack");
        lastAttackTime = Time.time - attackCooldown;
    }

    public override void UpdateState(BoglinController boglin)
    {
        if (boglin.player == null) return;

        float distance = Vector2.Distance(boglin.transform.position, boglin.player.position);

        if (distance > boglin.attackRange)
        {
            boglin.SwitchState(boglin.walkState);
            return;
        }

        // Ataca se passou o cooldown
        if (Time.time >= lastAttackTime + attackCooldown)
        {
            // Aqui você pode instanciar fumaça ou ataque
            if (boglin.SmokeSpawn != null)
            {
                // Exemplo: instanciando o ataque de fumaça
                // Instantiate(boglin.smokePrefab, boglin.SmokeSpawn.position, Quaternion.identity);
            }

            lastAttackTime = Time.time;
        }
    }

    public override void ExitState(BoglinController boglin)
    {
        // Reseta triggers se precisar
    }
}
