using UnityEngine;

[System.Serializable]
public class AttackTiming
{
    public float attackDuration = 1f;   // duração da animação
    public float hitStart = 0.3f;       // início do hit
    public float hitEnd = 0.6f;         // fim do hit
    public float projectileSpawn = 0.5f;// momento do projétil (0 se não usar)
}

public class NitroMortisBoss_NoAnimEvent : MonoBehaviour
{
    public Transform player;
    public Animator anim;

    [Header("Hitboxes")]
    public GameObject attack1Hitbox;
    public GameObject attack2Hitbox;

    [Header("Projectile")]
    public GameObject poisonProjectilePrefab;
    public Transform mouthSpawnPoint;

    [Header("Attack Timings")]
    public AttackTiming clawAttack;  
    public AttackTiming mouthAttack; 

    [Header("Ranges")]
    public float detectionRange = 8f;
    public float clawRange = 2.5f;

    [Header("Probabilidade")]
    [Range(0f,1f)] public float mouthChance = 0.18f;

    [Header("Cooldown")]
    public float attackCooldown = 2.5f;

    private float cooldownTimer = 0f;
    private bool isAttacking = false;
    private float attackTimer = 0f;
    private AttackTiming currentTiming;
    private string activeAnimation = "";

    private enum AttackType { None, Claw, Mouth }
    private AttackType currentAttack = AttackType.None;

    void Update()
    {
        cooldownTimer += Time.deltaTime;

        if (!isAttacking) IdleLogic(); else AttackLogic();

        FlipTowardsPlayer();
    }

    void IdleLogic()
    {
        float dist = Vector2.Distance(transform.position, player.position);
        if (dist <= detectionRange && cooldownTimer >= attackCooldown) DecideAttack(dist);
        else anim.Play("Idle_Boss_Notro");
    }

    void DecideAttack(float dist)
    {
        isAttacking = true;
        attackTimer = 0f;

        if (dist > clawRange || Random.value < mouthChance)
        {
            currentAttack = AttackType.Mouth;
            anim.Play("Attack_BossMortis2");
            currentTiming = mouthAttack;
        }
        else
        {
            currentAttack = AttackType.Claw;
            anim.Play("Attack_BossMortis");
            currentTiming = clawAttack;
        }

        activeAnimation = anim.GetCurrentAnimatorClipInfo(0)[0].clip.name;
    }

    void AttackLogic()
    {
        attackTimer += Time.deltaTime;

        // Ativa hitbox
        if (attackTimer >= currentTiming.hitStart && attackTimer < currentTiming.hitEnd)
        {
            if (currentAttack == AttackType.Claw && !attack1Hitbox.activeSelf) attack1Hitbox.SetActive(true);
            if (currentAttack == AttackType.Mouth && !attack2Hitbox.activeSelf) attack2Hitbox.SetActive(true);
        }

        // Desativa hitbox
        if (attackTimer >= currentTiming.hitEnd)
        {
            attack1Hitbox.SetActive(false);
            attack2Hitbox.SetActive(false);
        }

        // Spawn projétil boca
        if (currentAttack == AttackType.Mouth && attackTimer >= currentTiming.projectileSpawn && currentTiming.projectileSpawn > 0)
        {
            SpawnProjectile();
            currentTiming.projectileSpawn = -999f; // impede spawn duplicado
        }

        // Fim do ataque
        if (attackTimer >= currentTiming.attackDuration)
        {
            EndAttack();
        }
    }

    void EndAttack()
    {
        isAttacking = false;
        cooldownTimer = 0f;
        attackTimer = 0f;
        currentAttack = AttackType.None;

        attack1Hitbox.SetActive(false);
        attack2Hitbox.SetActive(false);
    }

    void SpawnProjectile()
    {
        if (!poisonProjectilePrefab || !mouthSpawnPoint || !player) return;

        GameObject proj = Instantiate(poisonProjectilePrefab, mouthSpawnPoint.position, Quaternion.identity);
        proj.GetComponent<PoisonProjectile>().SetDirection(player.position - mouthSpawnPoint.position);
    }

    void FlipTowardsPlayer()
    {
        if (player == null) return;

        Vector3 scale = transform.localScale;

        // Se o player estiver à esquerda, escala negativa no X; à direita, positiva
        if (player.position.x < transform.position.x)
            scale.x = -Mathf.Abs(scale.x);
        else
            scale.x = Mathf.Abs(scale.x);

        transform.localScale = scale;
    }
}
