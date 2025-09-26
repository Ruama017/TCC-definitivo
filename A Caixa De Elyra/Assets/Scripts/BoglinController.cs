using UnityEngine;

public class BoglinController : MonoBehaviour
{
    [Header("Movimento")]
    public float moveSpeed = 2f;
    public float attackRange = 1f;

    [Header("Player")]
    public Transform player;

    [Header("Dano")]
    public int damage = 1;
    public float attackCooldown = 1f;
    private float lastAttackTime;

    private Rigidbody2D rb;
    private Animator animator;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
    }

    void Update()
    {
        if (player == null) return;

        // Distância até o player
        float distance = Vector2.Distance(transform.position, player.position);

        if (distance > attackRange)
        {
            // Move em direção ao player
            Vector2 direction = (player.position - transform.position).normalized;
            rb.linearVelocity = direction * moveSpeed;

            // Ativa animação de andar se existir
            animator.SetBool("isWalking", true);
        }
        else
        {
            // Para de se mover
            rb.linearVelocity = Vector2.zero;
            animator.SetBool("isWalking", false);

            // Ataca se passou o cooldown
            if (Time.time - lastAttackTime >= attackCooldown)
            {
                Attack();
                lastAttackTime = Time.time;
            }
        }
    }

    void Attack()
    {
        // Aqui você aplica o dano no player
        PlayerHealth playerHealth = player.GetComponent<PlayerHealth>();
        if (playerHealth != null)
        {
            playerHealth.TakeDamage(damage);
        }

        // Toca animação de ataque, se tiver
        animator.SetTrigger("Attack");
    }
}
