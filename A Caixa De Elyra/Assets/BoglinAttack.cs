using UnityEngine;

public class BoglinAttack : StateMachineBehaviour
{
    public float attackCooldown = 1.5f;
    public int attackDamage = 1;

    float lastAttackTime;
    BoglinController boglin;

    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        boglin = animator.GetComponent<BoglinController>();
        lastAttackTime = Time.time - attackCooldown;
    }

    override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        if (Time.time >= lastAttackTime + attackCooldown)
        {
            PlayerHealth ph = boglin.player.GetComponent<PlayerHealth>();
            if (ph != null)
                ph.TakeDamage(attackDamage);

            if (boglin.attackSound != null)
                boglin.attackSound.Play();

            lastAttackTime = Time.time;
        }
    }
}
