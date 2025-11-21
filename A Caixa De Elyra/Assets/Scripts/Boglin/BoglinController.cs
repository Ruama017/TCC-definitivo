using UnityEngine;

public class BoglinController : MonoBehaviour
{
    [Header("Referências")]
    public Transform player;
    public Animator anim;
    public Rigidbody2D rb;

    [Header("Estados")]
    public BoglinBaseState currentState;
    public BoglinWalkState walkState;
    public BoglinAttackState attackState;
    public BoglinPatrolState patrolState;

    [Header("Configurações de Movimento")]
    public float moveSpeed = 2f;

    [Header("Detecção e Ataque")]
    public float detectionRange = 5f;
    public float attackRange = 1.5f;
    public Transform AttackPoint;
    public Transform SmokeSpawn;

    [Header("Patrulha")]
    public Transform leftPatrolPoint;
    public Transform rightPatrolPoint;

    [Header("Saúde e Alma")]
    public int maxHealth = 5;
    public int currentHealth;
    public GameObject boglinSoulPrefab;
    public MonsterCounter monsterCounter;

    // --- ADIÇÃO: sons ---
    [Header("Sons do Boglin")]
    public AudioSource attackSound;
    public AudioSource deathSound;
    // ---------------------

    void Start()
    {
        currentHealth = maxHealth;

        walkState = new BoglinWalkState();
        attackState = new BoglinAttackState();
        patrolState = new BoglinPatrolState(leftPatrolPoint.position, rightPatrolPoint.position);

        SwitchState(patrolState);
    }

    void FixedUpdate()
    {
        if (currentState != null)
            currentState.UpdateState(this);
    }

    public void SwitchState(BoglinBaseState newState)
    {
        if (currentState != null)
            currentState.ExitState(this);

        currentState = newState;

        if (currentState != null)
            currentState.EnterState(this);
    }

    public void MoveTowards(Vector3 target)
    {
        Vector2 newPos = Vector2.MoveTowards(rb.position, target, moveSpeed * Time.fixedDeltaTime);
        rb.MovePosition(newPos);

        if (anim != null)
            anim.SetBool("IsWalking", true);

        Vector2 direction = (target - transform.position).normalized;
        if (direction.x > 0)
            transform.localScale = new Vector3(-1, 1, 1);
        else if (direction.x < 0)
            transform.localScale = new Vector3(1, 1, 1);
    }

    public void StopMoving()
    {
        if (anim != null)
            anim.SetBool("IsWalking", false);
    }

    public void PlayAttackSound()
    {
        // --- ADIÇÃO: tocar som no ataque ---
        if (attackSound != null)
            attackSound.Play();
    }

    public void TakeDamage(int damage)
    {
        currentHealth -= damage;
        if (currentHealth <= 0)
            Die();
    }

    void Die()
    {
        // --- ADIÇÃO: som de morte ---
        if (deathSound != null)
            deathSound.Play();
        // -----------------------------

        if (boglinSoulPrefab != null && monsterCounter != null)
        {
            GameObject soul = Instantiate(boglinSoulPrefab, transform.position, Quaternion.identity);
            Soul soulScript = soul.GetComponent<Soul>();
            if (soulScript != null)
                soulScript.Initialize(monsterCounter.transform.position, monsterCounter);
        }

        Destroy(gameObject);
    }

    public BoglinWalkState GetWalkState() => walkState;
    public BoglinAttackState GetAttackState() => attackState;
    public BoglinPatrolState GetPatrolState() => patrolState;
}
