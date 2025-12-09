using UnityEngine;

public class BoglinAttackState : BoglinBaseState
{
    public float attackCooldown = 1.5f;
    private float lastAttackTime;
    public float attackRadius = 1f;
    public int attackDamage = 1;

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
            boglin.SwitchState(boglin.GetWalkState());
            return;
        }

        // Ataca se passou o cooldown
        if (Time.time >= lastAttackTime + attackCooldown)
        {
            // Debug
            Debug.Log("Boglin atacou!");

            // Acerta o player diretamente (não depende de layer)
            PlayerHealth ph = boglin.player.GetComponent<PlayerHealth>();
            if (ph != null)
            {
                ph.TakeDamage(attackDamage);
            }

            // ADIÇÃO: toca som de ataque
            if (boglin.attackSound != null)
                boglin.attackSound.Play();
           

            lastAttackTime = Time.time;

            // Aciona animação novamente
            if (boglin.anim != null)
                boglin.anim.SetTrigger("Attack");
        }
    }

    public override void ExitState(BoglinController boglin)
    {
        if (boglin.anim != null)
            boglin.anim.ResetTrigger("Attack");
    }
}
