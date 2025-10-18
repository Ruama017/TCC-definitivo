using UnityEngine;
using System.Collections;

public class BoglinController : MonoBehaviour
{
    [Header("Componentes")]
    public Animator animator;
    public Transform player;

    [Header("Patrulha")]
    public Transform[] patrolPoints;

    [Header("Distâncias")]
    public float chaseDistance = 5f;
    public float attackDistance = 1.5f;
    public float moveSpeed = 2f;

    [HideInInspector] public BoglinState currentState;
    [HideInInspector] public BoglinPatrolState patrolState;
    [HideInInspector] public BoglinChaseState chaseState;
    [HideInInspector] public BoglinAttackState attackState;
    [HideInInspector] public BoglinDeadState deadState;

    [Header("Vida")]
    public int maxHealth = 5;
    public int currentHealth;

    [Header("Alma")]
    public GameObject soulPrefab;
    public Transform soulTarget;
    public float soulSpeed = 300f;

    [HideInInspector] public Vector3 originalScale;

    void Start()
    {
        originalScale = transform.localScale;

        patrolState = new BoglinPatrolState(this);
        chaseState = new BoglinChaseState(this);
        attackState = new BoglinAttackState(this);
        deadState = new BoglinDeadState(this);

        currentState = patrolState;
        currentHealth = maxHealth;
    }

    void Update()
    {
        currentState.LogicUpdate();
    }

    public void ChangeState(BoglinState newState)
    {
        currentState.Exit();
        currentState = newState;
        currentState.Enter();
    }

    public void MoveTowards(Vector2 target)
    {
        Vector2 direction = (target - (Vector2)transform.position).normalized;
        transform.position = Vector2.MoveTowards(transform.position, target, moveSpeed * Time.deltaTime);

        if (direction.x != 0)
            transform.localScale = new Vector3(originalScale.x * Mathf.Sign(direction.x), originalScale.y, originalScale.z);
    }

    public void AttackPlayer()
    {
        if (Vector2.Distance(transform.position, player.position) <= attackDistance)
        {
            PlayerHealth ph = player.GetComponent<PlayerHealth>();
            if (ph != null)
                ph.TakeDamage(1);
        }
    }

    // --- NOVO: Receber dano do ataque do player ---
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("PlayerAttack"))
        {
            TakeDamage(1); // Desconta 1 de vida por ataque
        }
    }

    public void TakeDamage(int damage)
    {
        currentHealth -= damage;

        if (animator != null)
            animator.SetTrigger("Hit"); // Opcional: animação de hit

        if (currentHealth <= 0)
            Die();
    }

    private void Die()
    {
        ChangeState(deadState);

        // Instancia a alma
        if (soulPrefab != null && soulTarget != null)
        {
            GameObject soul = Instantiate(soulPrefab, transform.position, Quaternion.identity, transform.parent);
            StartCoroutine(MoveSoulToTarget(soul));
        }

        // Desativa o Boglin visual
        gameObject.SetActive(false);
    }

    private IEnumerator MoveSoulToTarget(GameObject soul)
    {
        while (Vector3.Distance(soul.transform.position, soulTarget.position) > 0.1f)
        {
            soul.transform.position = Vector3.MoveTowards(soul.transform.position, soulTarget.position, soulSpeed * Time.deltaTime);
            yield return null;
        }

        if (CounterManager.Instance != null)
            CounterManager.Instance.Increment();

        Destroy(soul);
    }
}