using UnityEngine;
using System.Collections;

public class NitroMortisBoss : MonoBehaviour
{
    public Transform player;
    public Animator anim;

    [Header("Hitboxes")]
    public GameObject clawAttackCollider; // garra
    public GameObject mouthAttackCollider; // boca

    [Header("Projectile")]
    public GameObject poisonProjectilePrefab;
    public Transform mouthSpawnPoint;

    [Header("Attack Timings")]
    public float clawHitStart = 0.2f;
    public float clawHitEnd = 0.5f;
    public float clawDuration = 1f;

    public float mouthHitStart = 0.3f;
    public float mouthHitEnd = 0.6f;
    public float mouthDuration = 1.5f;
    public float projectileSpawnTime = 0.5f;

    [Header("Ranges")]
    public float detectionRange = 20f; 
    public float clawRange = 6f;

    [Header("Health")]
    public int maxHealth = 5;
    private int currentHealth;

    [Header("Thorne")]
    public ThorneBossController thorneBoss; // só ativa depois da morte

    private bool isDead = false;
    private bool isAttacking = false;

    private enum AttackType { None, Claw, Mouth }
    private AttackType currentAttack = AttackType.None;
    private float attackTimer = 0f;

    void Start()
    {
        currentHealth = maxHealth;

        if (clawAttackCollider != null)
            clawAttackCollider.SetActive(false);
        if (mouthAttackCollider != null)
            mouthAttackCollider.SetActive(false);

        if (thorneBoss != null)
            thorneBoss.gameObject.SetActive(false); // só aparece depois
    }

    void Update()
    {
        if (isDead || player == null) return;

        float dist = Vector2.Distance(transform.position, player.position);

        if (!isAttacking)
        {
            if (dist <= clawRange)
                StartClawAttack();
            else if (dist <= detectionRange)
                StartMouthAttack();
        }

        if (isAttacking)
            AttackLogic();
    }

    // ===================== ATAQUES =====================
    void StartClawAttack()
    {
        isAttacking = true;
        currentAttack = AttackType.Claw;
        attackTimer = 0f;
        anim.SetTrigger("ClawAttack");
    }

    void StartMouthAttack()
    {
        isAttacking = true;
        currentAttack = AttackType.Mouth;
        attackTimer = 0f;
        anim.SetTrigger("MouthAttack");
    }

    void AttackLogic()
    {
        attackTimer += Time.deltaTime;

        if (currentAttack == AttackType.Claw)
        {
            if (attackTimer >= clawHitStart && attackTimer < clawHitEnd && clawAttackCollider != null)
                clawAttackCollider.SetActive(true);
            if (attackTimer >= clawHitEnd && clawAttackCollider != null)
                clawAttackCollider.SetActive(false);
            if (attackTimer >= clawDuration)
                EndAttack();
        }

        if (currentAttack == AttackType.Mouth)
        {
            if (attackTimer >= mouthHitStart && attackTimer < mouthHitEnd && mouthAttackCollider != null)
                mouthAttackCollider.SetActive(true);
            if (attackTimer >= mouthHitEnd && mouthAttackCollider != null)
                mouthAttackCollider.SetActive(false);
            if (attackTimer >= projectileSpawnTime)
            {
                SpawnProjectile();
                projectileSpawnTime = 999f; // garante só 1 vez
            }
            if (attackTimer >= mouthDuration)
                EndAttack();
        }
    }

    void EndAttack()
    {
        isAttacking = false;
        currentAttack = AttackType.None;
        attackTimer = 0f;

        if (clawAttackCollider != null)
            clawAttackCollider.SetActive(false);
        if (mouthAttackCollider != null)
            mouthAttackCollider.SetActive(false);

        projectileSpawnTime = 0.5f; // reseta para próximo ataque
    }

    void SpawnProjectile()
    {
        if (poisonProjectilePrefab != null && mouthSpawnPoint != null && player != null)
        {
            GameObject proj = Instantiate(poisonProjectilePrefab, mouthSpawnPoint.position, Quaternion.identity);
            proj.GetComponent<PoisonProjectile>().SetDirection(player.position - mouthSpawnPoint.position);
        }
    }

    // ===================== DANO =====================
    public void TakeDamage(bool superActive)
    {
        if (!superActive || isDead) return;

        currentHealth -= 1;

        if (currentHealth <= 0)
            StartCoroutine(DeathEffect());
    }

    IEnumerator DeathEffect()
    {
        isDead = true;
        anim.enabled = false;

        // opcional: efeito visual de morte
        yield return new WaitForSeconds(0.5f);

        // ativa o Thorne
        if (thorneBoss != null)
            thorneBoss.gameObject.SetActive(true);

        // destrói o NitroMortis depois de 0.5s
        Destroy(gameObject, 0.5f);
    }
}
