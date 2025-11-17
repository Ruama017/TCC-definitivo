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
    public int maxHealth = 10;
    private int currentHealth;

    [Header("Ataque")]
    public GameObject attackHitbox;
    public float attackDuration = 0.3f;

    [Header("Som")]
    public AudioSource deathSound;

    [Header("Referência do Sprite")]
    public SpriteRenderer sr;

    private bool isDead = false;
    private bool facingLeft = true; // seu sprite olha para a ESQUERDA

    // ====================== ATIVAR BOSS ======================
    private bool isActive = false;

    public void ActivateBoss()
    {
        isActive = true;
        teleportTimer = 0f;
        anim.SetBool("Walking", false);
    }

    void OnEnable()
    {
        isActive = false; // começa desligado
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

        // TELEPORTE
        if (teleportTimer >= teleportInterval)
        {
            teleportTimer = 0f;
            StartCoroutine(Teleport());
        }

        // MOVIMENTO
        float dist = Vector2.Distance(transform.position, player.position);

        if (dist > attackRange)
        {
            MoveTowardsPlayer();
        }
        else
        {
            Attack();
        }

        FlipTowardsPlayer();
    }

    // =========================== MOVIMENTO ===========================
    void MoveTowardsPlayer()
    {
        anim.SetBool("Walking", true);

        Vector2 target = new Vector2(player.position.x, transform.position.y);
        transform.position = Vector2.MoveTowards(transform.position, target, speed * Time.deltaTime);
    }

    // ============================ ATAQUE =============================
    void Attack()
    {
        anim.SetBool("Walking", false);

        if (!anim.GetCurrentAnimatorStateInfo(0).IsName("Attack_Thorne"))
        {
            anim.SetTrigger("Attack");
            StartCoroutine(ActivateAttackHitbox());
        }
    }

    IEnumerator ActivateAttackHitbox()
    {
        yield return new WaitForSeconds(0.1f);
        attackHitbox.SetActive(true);

        yield return new WaitForSeconds(attackDuration);
        attackHitbox.SetActive(false);
    }

    // ======================= TELEPORTE COMPLETO ======================
    IEnumerator Teleport()
    {
        // fade out
        yield return StartCoroutine(Fade(0f));

        // teleporta atrás do player
        Vector3 newPos = player.position;
        if (facingLeft)
            newPos.x -= 2f; // aparece atrás
        else
            newPos.x += 2f;

        transform.position = newPos;

        // fade in
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

        if (dir > 0 && facingLeft) // player à direita
        {
            facingLeft = false;
            transform.localScale = new Vector3(-1, 1, 1);
        }
        else if (dir < 0 && !facingLeft) // player à esquerda
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
            amount *= 3; // SUPER dá 3x mais dano

        currentHealth -= amount;

        if (currentHealth <= 0)
        {
            StartCoroutine(DeathEffect());
        }
    }

    IEnumerator DeathEffect()
    {
        isDead = true;

        if (deathSound != null)
            deathSound.Play();

        // piscar
        for (int i = 0; i < 6; i++)
        {
            sr.enabled = !sr.enabled;
            yield return new WaitForSeconds(0.15f);
        }

        // desaparecer
        yield return StartCoroutine(Fade(0f));

        Destroy(gameObject);
    }
}
