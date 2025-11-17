using UnityEngine;
using System.Collections;

public class ThorneBossController : MonoBehaviour
{
    public Transform player;
    public Animator anim;
    public float speed = 3f;
    public float attackRange = 2f;

    [Header("Teleport")]
    public float teleportInterval = 15f;
    public float fadeDuration = 0.5f;
    private float teleportTimer = 0f;

    [Header("Vida")]
    public int maxHealth = 50;   // AGORA MUITO MAIS VIDA
    private int currentHealth;

    [Header("Ataque")]
    public GameObject attackHitbox;
    public float attackCooldown = 0.8f; // intervalo entre ataques
    public float attackDuration = 0.3f;
    private bool isAttacking = false;

    [Header("Som")]
    public AudioSource deathSound;

    [Header("Referência do Sprite")]
    public SpriteRenderer sr;

    private bool isDead = false;
    private bool facingLeft = true;
    private bool isActive = false;

    public void ActivateBoss()
    {
        isActive = true;
        teleportTimer = 0f;
    }

    void OnEnable()
    {
        isActive = false;
    }

    void Start()
    {
        currentHealth = maxHealth;

        if (attackHitbox != null)
            attackHitbox.SetActive(false);
    }

    void Update()
    {
        if (isDead || player == null || !isActive) return;

        teleportTimer += Time.deltaTime;

        if (teleportTimer >= teleportInterval)
        {
            teleportTimer = 0f;
            StartCoroutine(Teleport());
        }

        float dist = Vector2.Distance(transform.position, player.position);

        if (dist > attackRange)
        {
            anim.SetBool("Walking", true);
            MoveTowardsPlayer();
        }
        else
        {
            anim.SetBool("Walking", false);
            TryAttack();
        }

        FlipTowardsPlayer();
    }

    // =========================== MOVIMENTO ===========================
    void MoveTowardsPlayer()
    {
        Vector2 target = new Vector2(player.position.x, transform.position.y);
        transform.position = Vector2.MoveTowards(transform.position, target, speed * Time.deltaTime);
    }

    // ============================ ATAQUE =============================
    void TryAttack()
    {
        if (!isAttacking)
            StartCoroutine(Attack());
    }

    IEnumerator Attack()
    {
        isAttacking = true;

        anim.SetTrigger("Attack");
        yield return new WaitForSeconds(0.1f);
        attackHitbox.SetActive(true);

        yield return new WaitForSeconds(attackDuration);
        attackHitbox.SetActive(false);

        yield return new WaitForSeconds(attackCooldown);

        isAttacking = false;
    }

    // ======================= TELEPORTE COMPLETO ======================
    IEnumerator Teleport()
    {
        yield return StartCoroutine(Fade(0f));

        Vector3 newPos = player.position;
        newPos.x += facingLeft ? -2f : 2f;
        transform.position = newPos;

        yield return StartCoroutine(Fade(1f));
    }

    IEnumerator Fade(float targetAlpha)
    {
        float startAlpha = sr.color.a;
        float t = 0;

        while (t < fadeDuration)
        {
            t += Time.deltaTime;
            float a = Mathf.Lerp(startAlpha, targetAlpha, t / fadeDuration);
            sr.color = new Color(sr.color.r, sr.color.g, sr.color.b, a);
            yield return null;
        }
    }

    // ============================ FLIP ===============================
    void FlipTowardsPlayer()
    {
        float dir = player.position.x - transform.position.x;

        if (dir > 0 && facingLeft)
        {
            facingLeft = false;
            transform.localScale = new Vector3(-1, 1, 1);
        }
        else if (dir < 0 && !facingLeft)
        {
            facingLeft = true;
            transform.localScale = new Vector3(1, 1, 1);
        }
    }

    // ===================== DANO E MORTE =============================
    public void TakeDamage(int amount, bool superActive)
    {
        if (isDead) return;

        if (superActive)
            amount *= 3;

        currentHealth -= amount;

        if (currentHealth <= 0)
            StartCoroutine(DeathEffect());
    }

    IEnumerator DeathEffect()
    {
        isDead = true;

        if (deathSound != null)
            deathSound.Play();

        for (int i = 0; i < 6; i++)
        {
            sr.enabled = !sr.enabled;
            yield return new WaitForSeconds(0.15f);
        }

        yield return StartCoroutine(Fade(0f));

        Destroy(gameObject);
    }
}
