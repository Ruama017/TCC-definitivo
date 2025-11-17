using UnityEngine;

public class ThorneBossController : MonoBehaviour
{
    [Header("Player")]
    public Transform player;

    [Header("Stats")]
    public int maxHealth = 50;
    private int currentHealth;

    public float speed = 2.5f;
    public float attackRange = 2f;
    public int damage = 3;

    [Header("Animator")]
    private Animator anim;

    [Header("Attack Hitbox")]
    public GameObject attackHitbox;   // <<< arraste aqui o AttackHitbox no inspetor

    private bool isAttacking = false;
    private bool isTeleporting = false;

    private float nextTeleportTime = 0f;
    public float teleportCooldown = 15f;

    private bool isDead = false;

    void Start()
    {
        currentHealth = maxHealth;
        anim = GetComponent<Animator>();

        if (attackHitbox != null)
            attackHitbox.SetActive(false); // garante que comece desligada
    }

    void Update()
    {
        if (isDead) return;
        if (player == null) return;

        HandleTeleport();
        HandleCombat();
    }

    void HandleCombat()
    {
        if (isTeleporting) return;

        float distance = Vector2.Distance(transform.position, player.position);

        if (distance <= attackRange)
            StartAttack();
        else
            WalkTowardsPlayer();

        FlipTowardsPlayer();
    }

    void WalkTowardsPlayer()
    {
        anim.SetBool("attack", false);
        anim.SetBool("isWalking", true);

        Vector2 dir = (player.position - transform.position).normalized;
        transform.position += (Vector3)(dir * speed * Time.deltaTime);
    }

    void StartAttack()
    {
        if (isAttacking) return;

        isAttacking = true;
        anim.SetBool("isWalking", false);
        anim.SetBool("attack", true);

        StartCoroutine(AttackRoutine());
    }

    System.Collections.IEnumerator AttackRoutine()
    {
        // Liga a hitbox no início do ataque
        if (attackHitbox != null)
            attackHitbox.SetActive(true);

        float animTime = anim.GetCurrentAnimatorStateInfo(0).length;

        yield return new WaitForSeconds(animTime);

        // Desliga a hitbox quando o ataque acaba
        if (attackHitbox != null)
            attackHitbox.SetActive(false);

        anim.SetBool("attack", false);
        isAttacking = false;
    }

    void HandleTeleport()
    {
        if (Time.time >= nextTeleportTime)
        {
            nextTeleportTime = Time.time + teleportCooldown;
            StartCoroutine(TeleportRoutine());
        }
    }

    System.Collections.IEnumerator TeleportRoutine()
    {
        isTeleporting = true;
        anim.SetTrigger("teleport");

        yield return new WaitForSeconds(0.4f);

        Vector3 offset = new Vector3(2.5f * Mathf.Sign(player.localScale.x), 0, 0);
        transform.position = player.position - offset;

        yield return new WaitForSeconds(0.2f);

        isTeleporting = false;
    }

    void FlipTowardsPlayer()
    {
        if (player.position.x < transform.position.x)
            transform.localScale = new Vector3(1, 1, 1);
        else
            transform.localScale = new Vector3(-1, 1, 1);
    }

    public void TakeDamage(int dmg)
    {
        if (isDead) return;

        currentHealth -= dmg;

        if (currentHealth <= 0)
            Die();
    }

    void Die()
    {
        isDead = true;
        anim.SetBool("attack", false);
        anim.SetBool("isWalking", false);
        Destroy(gameObject, 3f);
    }
}
