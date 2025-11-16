using UnityEngine;
using System.Collections;

public class NitroMortisBoss : MonoBehaviour
{
    public Transform player;
    public Animator anim;

    [Header("Hitboxes")]
    public GameObject attack1Hitbox; // Garra
    public GameObject attack2Hitbox; // Boca

    [Header("Projectile")]
    public GameObject poisonProjectilePrefab;
    public Transform mouthSpawnPoint;

    [Header("Attack Timings")]
    public AttackTiming clawAttack;
    public AttackTiming mouthAttack;

    [Header("Ranges")]
    public float detectionRange = 20f;
    public float clawRange = 6f;

    [Header("Cooldown")]
    public float minCooldown = 0.2f;

    [Header("Thorne")]
    public ThorneBossController thorne; // Arraste o Thorne aqui

    [Header("Vida")]
    public int maxHealth = 15;
    private int currentHealth;

    [Header("Alma e efeitos de morte")]
    public GameObject soulPrefab;
    public float fadeDuration = 1f;
    public int blinkCount = 3;

    private bool isAttacking = false;
    private float attackTimer = 0f;
    private AttackTiming currentTiming;
    private enum AttackType { None, Claw, Mouth }
    private AttackType currentAttack = AttackType.None;

    private bool facingLeft = true;
    private bool isDead = false;

    private SpriteRenderer sr;

    void Start()
    {
        currentHealth = maxHealth;
        sr = GetComponent<SpriteRenderer>();
    }

    void Update()
    {
        if (isDead || !player) return;

        float dist = Vector2.Distance(transform.position, player.position);

        if (!isAttacking && dist <= detectionRange)
            DecideAttack(dist);

        if (isAttacking)
            AttackLogic();

        FlipTowardsPlayer();
    }

    void DecideAttack(float dist)
    {
        isAttacking = true;
        attackTimer = 0f;

        if (dist <= clawRange)
        {
            currentAttack = AttackType.Claw;
            currentTiming = clawAttack;
            anim.SetTrigger("ClawAttack");
        }
        else
        {
            currentAttack = AttackType.Mouth;
            currentTiming = mouthAttack;
            anim.SetTrigger("MouthAttack");
        }
    }

    void AttackLogic()
    {
        attackTimer += Time.deltaTime;

        // Ativa hitbox
        if (attackTimer >= currentTiming.hitStart && attackTimer < currentTiming.hitEnd)
        {
            if (currentAttack == AttackType.Claw && !attack1Hitbox.activeSelf)
                attack1Hitbox.SetActive(true);

            if (currentAttack == AttackType.Mouth && !attack2Hitbox.activeSelf)
                attack2Hitbox.SetActive(true);
        }

        // Desativa hitboxes
        if (attackTimer >= currentTiming.hitEnd)
        {
            attack1Hitbox.SetActive(false);
            attack2Hitbox.SetActive(false);
        }

        // Spawn projétil
        if (currentAttack == AttackType.Mouth &&
            attackTimer >= currentTiming.projectileSpawn &&
            currentTiming.projectileSpawn > 0)
        {
            SpawnProjectile();
            currentTiming.projectileSpawn = -999f;
        }

        // Fim do ataque
        if (attackTimer >= currentTiming.attackDuration)
            EndAttack();
    }

    void EndAttack()
    {
        isAttacking = false;
        attackTimer = 0f;
        currentAttack = AttackType.None;

        attack1Hitbox.SetActive(false);
        attack2Hitbox.SetActive(false);

        mouthAttack.projectileSpawn = 0.5f;
    }

    void SpawnProjectile()
    {
        if (!poisonProjectilePrefab || !mouthSpawnPoint || !player) return;

        GameObject proj = Instantiate(poisonProjectilePrefab, mouthSpawnPoint.position, Quaternion.identity);
        proj.GetComponent<PoisonProjectile>().SetDirection(player.position - mouthSpawnPoint.position);
    }

    void FlipTowardsPlayer()
    {
        float direction = player.position.x - transform.position.x;
        if (direction > 0 && facingLeft) Flip();
        else if (direction < 0 && !facingLeft) Flip();
    }

    void Flip()
    {
        facingLeft = !facingLeft;
        Vector3 scale = transform.localScale;
        scale.x *= -1;
        transform.localScale = scale;
    }

    // ===================== VIDA & MORTE =====================
    public void TakeDamage(int damage, bool isSuper)
    {
        if (isDead) return;

        int dmg = isSuper ? damage * 3 : damage; // Mais dano se player tiver Super
        currentHealth -= dmg;

        if (currentHealth <= 0)
            StartCoroutine(DieCoroutine());
    }

    private IEnumerator DieCoroutine()
    {
        isDead = true;
        anim.SetTrigger("Death");

        // Spawn da alma
        if (soulPrefab != null)
            Instantiate(soulPrefab, transform.position, Quaternion.identity);

        // Pisca 3 vezes
        for (int i = 0; i < blinkCount; i++)
        {
            sr.enabled = false;
            yield return new WaitForSeconds(0.2f);
            sr.enabled = true;
            yield return new WaitForSeconds(0.2f);
        }

        // Fade antes de sumir
        float timer = 0f;
        Color originalColor = sr.color;
        while (timer < fadeDuration)
        {
            timer += Time.deltaTime;
            sr.color = new Color(originalColor.r, originalColor.g, originalColor.b, Mathf.Lerp(1, 0, timer / fadeDuration));
            yield return null;
        }

        sr.color = originalColor;

        // Libera o Thorne
        if (thorne != null)
            thorne.ActivateBoss();

        gameObject.SetActive(false);
        Debug.Log("[DEBUG] NitroMortis MORREU!");
    }
}
