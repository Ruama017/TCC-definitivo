using UnityEngine;

public class NitroMortisBoss : MonoBehaviour
{
    public Transform player;
    public Animator anim;

    [Header("Hitboxes")]
    public GameObject attack1Hitbox; // garra
    public GameObject attack2Hitbox; // boca

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
    public ThorneBossController thorne; // arraste o Thorne aqui

    [Header("Vida")]
    public int maxHealth = 15;
    private int currentHealth;

    private bool isAttacking = false;
    private float attackTimer = 0f;
    private AttackTiming currentTiming;
    private enum AttackType { None, Claw, Mouth }
    private AttackType currentAttack = AttackType.None;

    private bool facingLeft = true;

    void Start()
    {
        currentHealth = maxHealth;
    }

    void Update()
    {
        if (!player) return;

        float dist = Vector2.Distance(transform.position, player.position);

        // Decide atacar mesmo se player estiver à esquerda ou direita
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
        int dmg = isSuper ? damage * 3 : damage; // mais dano se player tiver Super
        currentHealth -= dmg;

        if (currentHealth <= 0)
            Die();
    }

    private void Die()
    {
        gameObject.SetActive(false);

        // Libera o Thorne
        if (thorne != null)
            thorne.ActivateBoss();
    }
}
