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

    [Header("Sons do Boglin")]
    public AudioSource attackSound;
    public AudioSource deathSound;

    void Start()
    {
        currentHealth = maxHealth;

        walkState = new BoglinWalkState();
        attackState = new BoglinAttackState();
        patrolState = new BoglinPatrolState(
            leftPatrolPoint.position,
            rightPatrolPoint.position,
            transform.position.y
        );

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
        // só mexe no X
        Vector2 targetPos = new Vector2(target.x, rb.position.y);
        Vector2 newPos = Vector2.MoveTowards(rb.position, targetPos, moveSpeed * Time.fixedDeltaTime);
        rb.MovePosition(newPos);

        if (anim != null)
            anim.SetBool("IsWalking", true);

        // flip horizontal
        if (target.x > transform.position.x)
            transform.localScale = new Vector3(-1, 1, 1);
        else
            transform.localScale = new Vector3(1, 1, 1);
    }

    public void StopMoving()
    {
        if (anim != null)
            anim.SetBool("IsWalking", false);
    }

    public void PlayAttackSound()
    {
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
        if (deathSound != null)
            deathSound.Play();

        if (boglinSoulPrefab != null)
            Instantiate(boglinSoulPrefab, transform.position, Quaternion.identity);

        Destroy(gameObject);
    }

    public BoglinWalkState GetWalkState() => walkState;
    public BoglinAttackState GetAttackState() => attackState;
    public BoglinPatrolState GetPatrolState() => patrolState;
}
