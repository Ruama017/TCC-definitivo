using UnityEngine;

public class VoglinController : MonoBehaviour
{
    [Header("Movement")]
    [SerializeField] private float speed = 3f;
    [SerializeField] private float followRange = 6f;
    [SerializeField] private float attackRange = 2.5f;

    [Header("Patrol")]
    [SerializeField] private Transform[] patrolPoints;
    [SerializeField] private float patrolWaitTime = 1.0f;
    private int currentPatrolIndex = 0;
    private float patrolTimer = 0f;

    [Header("Attack")]
    [SerializeField] private GameObject projectilePrefab;
    [SerializeField] private Transform projectileSpawn;
    [SerializeField] private float attackCooldown = 2.5f;
    private float attackTimer = 0f;

    [Header("Health & Soul")]
    [SerializeField] private int maxHealth = 3;
    private int currentHealth;
    [SerializeField] private GameObject soulPrefab;
    [SerializeField] private Transform soulSpawnPoint;

    [Header("Audio")]
    [SerializeField] private AudioClip deathSound;
    private AudioSource audioSource;

    private Transform player;
    private Animator anim;
    private bool facingRight = true;
    private bool isDead = false;

    private enum VoglinState { Patrol, Follow, Attack }
    private VoglinState currentState = VoglinState.Patrol;

    private void Awake()
    {
        anim = GetComponent<Animator>();
        audioSource = GetComponent<AudioSource>();
        currentHealth = maxHealth;

        GameObject p = GameObject.FindGameObjectWithTag("Player");
        if (p != null) player = p.transform;
    }

    private void Update()
    {
        if (isDead) return;

        // TESTE: causar dano apertando K
        if (Input.GetKeyDown(KeyCode.K))
        {
            TakeDamage(1);
        }

        if (player == null)
        {
            GameObject p = GameObject.FindGameObjectWithTag("Player");
            if (p != null) player = p.transform;
        }

        attackTimer -= Time.deltaTime;

        float distance = player != null ? Vector2.Distance(transform.position, player.position) : Mathf.Infinity;

        if (distance <= attackRange)
            currentState = VoglinState.Attack;
        else if (distance <= followRange)
            currentState = VoglinState.Follow;
        else
            currentState = VoglinState.Patrol;

        switch (currentState)
        {
            case VoglinState.Patrol:
                Patrol();
                anim.SetBool("isFlying", true);
                anim.SetBool("isAttacking", false);
                break;

            case VoglinState.Follow:
                FollowPlayer();
                anim.SetBool("isFlying", true);
                anim.SetBool("isAttacking", false);
                break;

            case VoglinState.Attack:
                AttackPlayer();
                break;
        }
    }

    private void Patrol()
    {
        if (patrolPoints == null || patrolPoints.Length < 2) return;

        Transform targetPoint = patrolPoints[currentPatrolIndex];
        float dirX = targetPoint.position.x - transform.position.x;
        float moveDir = Mathf.Sign(dirX);

        if (moveDir > 0 && !facingRight) Flip();
        else if (moveDir < 0 && facingRight) Flip();

        transform.position = Vector2.MoveTowards(
            transform.position,
            targetPoint.position,
            speed * Time.deltaTime
        );

        if (Mathf.Abs(dirX) <= 0.05f)
        {
            patrolTimer += Time.deltaTime;
            if (patrolTimer >= patrolWaitTime)
            {
                patrolTimer = 0f;
                currentPatrolIndex = (currentPatrolIndex + 1) % patrolPoints.Length;
            }
        }
    }

    private void FollowPlayer()
    {
        if (player == null) return;

        Vector2 direction = (player.position - transform.position).normalized;

        if (direction.x > 0 && !facingRight) Flip();
        else if (direction.x < 0 && facingRight) Flip();

        transform.position = Vector2.MoveTowards(
            transform.position,
            player.position,
            speed * Time.deltaTime
        );
    }

    private void AttackPlayer()
    {
        if (player == null) return;

        anim.SetBool("isFlying", false);
        anim.SetBool("isAttacking", true);

        Vector2 direction = (player.position - transform.position).normalized;

        if (direction.x > 0 && !facingRight) Flip();
        else if (direction.x < 0 && facingRight) Flip();

        if (attackTimer <= 0f)
        {
            attackTimer = attackCooldown;
            FireProjectile(direction);
        }
    }

    private void FireProjectile(Vector2 dir)
    {
        if (projectilePrefab == null || projectileSpawn == null) return;

        GameObject proj = Instantiate(projectilePrefab, projectileSpawn.position, Quaternion.identity);
        VoglinProjectile vp = proj.GetComponent<VoglinProjectile>();
        if (vp != null) vp.SetDirection(dir);

        proj.transform.right = dir;
    }

    public void TakeDamage(int damage)
    {
        if (isDead) return;

        currentHealth -= damage;
        Debug.Log("Voglin recebeu dano! Vida atual: " + currentHealth);

        if (currentHealth <= 0)
            Die();
    }

    private void Die()
    {
        isDead = true;
        anim.SetTrigger("Die"); // 🎬 toca a animação de morte

        // 🎧 toca o som de morte
        if (deathSound != null && audioSource != null)
            audioSource.PlayOneShot(deathSound);

        // 👻 instancia a alma
        if (soulPrefab != null && soulSpawnPoint != null)
            Instantiate(soulPrefab, soulSpawnPoint.position, Quaternion.identity);

        // 🕒 destrói o inimigo após o som/ animação
        Destroy(gameObject, 1.5f);
    }

    private void Flip()
    {
        facingRight = !facingRight;
        Vector3 s = transform.localScale;
        s.x *= -1f;
        transform.localScale = s;
    }
}
