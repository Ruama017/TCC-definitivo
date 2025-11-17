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
    public int maxHealth = 50; // Thorne é mais forte
    private int currentHealth;

    [Header("Ataque")]
    public GameObject attackHitbox;
    public float attackDuration = 0.3f;

    [Header("Som")]
    public AudioSource deathSound;

    [Header("Referência do Sprite")]
    public SpriteRenderer sr;

    private bool isDead = false;
    private bool canTeleport = true;
    private bool facingLeft = true; // sprite olha para esquerda

    void Start()
    {
        currentHealth = maxHealth;

        if (attackHitbox != null)
            attackHitbox.SetActive(false);

        // começa invisível até NitroMortis morrer
        gameObject.SetActive(false);
    }

    void Update()
    {
        if (isDead || player == null) return;

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

    void MoveTowardsPlayer()
    {
        anim.SetBool("isWalking", true);
        Vector2 target = new Vector2(player.position.x, transform.position.y);
        transform.position = Vector2.MoveTowards(transform.position, target, speed * Time.deltaTime);
    }

    void Attack()
    {
        anim.SetBool("isWalking", false);

        if (!anim.GetCurrentAnimatorStateInfo(0).IsName("Attack_Thorne"))
        {
            anim.SetTrigger("attack");
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

    // ======= TakeDamage ajustado =======
    public void TakeDamage(int amount, bool superActive)
    {
        if (isDead) return;

        if (superActive) amount *= 3;

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

    // ======= Ativa o Thorne após NitroMortis morrer =======
    public void ActivateBoss()
    {
        gameObject.SetActive(true);
    }
}
