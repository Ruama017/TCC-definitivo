using UnityEngine;
using System.Collections;

public class ThorneBossController : MonoBehaviour
{
    [Header("Player")]
    public Transform player;

    [Header("Movimento")]
    public float moveSpeed = 3f;
    public float attackRange = 2f;

    [Header("Ataque")]
    public Animator anim;
    public GameObject attackHitbox;
    public float attackDuration = 0.3f;
    public int attackDamage = 1;
    public int superAttackDamage = 3;
    private bool isAttacking = false;

    [Header("Vida")]
    public int maxHealth = 20;
    private int currentHealth;

    [Header("Teleporte")]
    public float teleportInterval = 5f;

    [Header("Morte")]
    public AudioSource deathSound;
    public float fadeDuration = 1f;

    private bool active = false;
    private Rigidbody2D rb;
    private SpriteRenderer sr;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        sr = GetComponent<SpriteRenderer>();
        anim = GetComponent<Animator>();
        gameObject.SetActive(false);
        currentHealth = maxHealth;
    }

    void Update()
    {
        if (!active || isAttacking) return;

        FollowPlayer();

        float distance = Vector2.Distance(transform.position, player.position);
        if (distance <= attackRange)
            Attack();
    }

    void FollowPlayer()
    {
        if (player == null) return;

        float distance = Vector2.Distance(transform.position, player.position);
        if (distance > attackRange)
        {
            Vector2 dir = (player.position - transform.position).normalized;
            rb.velocity = new Vector2(dir.x * moveSpeed, rb.velocity.y);

            if (anim != null)
                anim.SetBool("isWalking", true);

            if (dir.x > 0) transform.localScale = new Vector3(1, 1, 1);
            else transform.localScale = new Vector3(-1, 1, 1);
        }
        else
        {
            rb.velocity = Vector2.zero;
            if (anim != null)
                anim.SetBool("isWalking", false);
        }
    }

    public void Attack()
    {
        if (isAttacking) return;

        isAttacking = true;
        if (anim != null)
            anim.SetTrigger("Attack");

        if (attackHitbox != null)
        {
            attackHitbox.SetActive(true);
            StartCoroutine(DisableHitboxAfterDelay(attackDuration));
        }
    }

    private IEnumerator DisableHitboxAfterDelay(float delay)
    {
        yield return new WaitForSeconds(delay);

        if (attackHitbox != null)
        {
            Collider2D[] hits = Physics2D.OverlapBoxAll(attackHitbox.transform.position, attackHitbox.transform.localScale, 0f);
            foreach (Collider2D hit in hits)
            {
                if (hit.CompareTag("Player"))
                {
                    PlayerController playerCtrl = hit.GetComponent<PlayerController>();
                    if (playerCtrl != null)
                    {
                        int dmg = playerCtrl.hasSuper ? superAttackDamage : attackDamage;
                        playerCtrl.TakeDamage(dmg);
                    }
                }
            }

            attackHitbox.SetActive(false);
        }

        isAttacking = false;
    }

    // ===================== TELEPORTE =====================
    public void ActivateBoss()
    {
        gameObject.SetActive(true);
        active = true;
        currentHealth = maxHealth;
        StartCoroutine(TeleportRoutine());
    }

    IEnumerator TeleportRoutine()
    {
        while (active)
        {
            yield return new WaitForSeconds(teleportInterval);
            TeleportToRandomPosition();
        }
    }

    void TeleportToRandomPosition()
    {
        Vector3 newPos = new Vector3(Random.Range(-8f, 8f), Random.Range(-4f, 4f), 0f);
        transform.position = newPos;
    }

    // ===================== VIDA & MORTE =====================
    public void TakeDamage(int damage, bool isSuper)
    {
        int dmg = isSuper ? superAttackDamage : damage;
        currentHealth -= dmg;

        if (currentHealth <= 0)
            StartCoroutine(Die());
    }

    private IEnumerator Die()
    {
        active = false;
        rb.velocity = Vector2.zero;
        isAttacking = true;

        if (deathSound != null)
            deathSound.Play();

        float t = 0f;
        Color originalColor = sr.color;

        while (t < fadeDuration)
        {
            t += Time.deltaTime;
            float alpha = Mathf.Lerp(1f, 0f, t / fadeDuration);
            sr.color = new Color(originalColor.r, originalColor.g, originalColor.b, alpha);
            yield return null;
        }

        gameObject.SetActive(false);
    }
}
