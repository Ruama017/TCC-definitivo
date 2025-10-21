using UnityEngine;

public class BoglinController : MonoBehaviour
{
    [Header("Componentes")]
    public Animator animator;
    public Transform player;

    [Header("Movimento")]
    public float moveSpeed = 2f;
    public float chaseDistance = 5f;
    public float attackDistance = 1.5f;

    [Header("Ataque")]
    public GameObject smokePrefab; // prefab da fumaça
    public Transform smokeSpawnPoint; // posição da boca
    private bool isAttacking = false;

    [Header("Vida e Alma")]
    public int maxHealth = 5;
    private int currentHealth;
    public GameObject soulPrefab;
    public Transform soulTarget;
    public float soulSpeed = 300f;

    private void Start()
    {
        currentHealth = maxHealth;
    }

    private void Update()
    {
        if (player == null) return;

        float distance = Vector2.Distance(transform.position, player.position);

        if (!isAttacking)
        {
            if (distance <= attackDistance)
            {
                Attack();
            }
            else if (distance <= chaseDistance)
            {
                MoveTowardsPlayer();
            }
            else
            {
                animator.SetBool("IsWalking", false);
            }
        }
    }

    void MoveTowardsPlayer()
    {
        animator.SetBool("IsWalking", true);
        Vector2 dir = (player.position - transform.position).normalized;
        transform.position = Vector2.MoveTowards(transform.position, player.position, moveSpeed * Time.deltaTime);
        if (dir.x != 0)
            transform.localScale = new Vector3(Mathf.Sign(dir.x) * Mathf.Abs(transform.localScale.x), transform.localScale.y, 1);
    }

    void Attack()
    {
        isAttacking = true;
        animator.SetTrigger("IsAttacking");

        // Spawn da fumaça
        if (smokePrefab != null && smokeSpawnPoint != null)
        {
            GameObject smoke = Instantiate(smokePrefab, smokeSpawnPoint.position, Quaternion.identity);
            SmokeAttack sa = smoke.GetComponent<SmokeAttack>();
            if (sa != null)
                sa.Init(player);
        }

        // Tempo do ataque baseado na animação
        Invoke(nameof(EndAttack), 1f); // ajuste conforme duração da animação
    }

    void EndAttack()
    {
        isAttacking = false;
    }

    public void TakeDamage(int damage)
    {
        currentHealth -= damage;
        if (currentHealth <= 0)
            Die();
    }

    void Die()
    {
        // Spawn da alma
        if (soulPrefab != null && soulTarget != null)
        {
            GameObject soul = Instantiate(soulPrefab, transform.position, Quaternion.identity);
            StartCoroutine(MoveSoulToTarget(soul));
        }

        Destroy(gameObject);
    }

    private System.Collections.IEnumerator MoveSoulToTarget(GameObject soul)
    {
        while (Vector3.Distance(soul.transform.position, soulTarget.position) > 0.1f)
        {
            soul.transform.position = Vector3.MoveTowards(soul.transform.position, soulTarget.position, soulSpeed * Time.deltaTime);
            yield return null;
        }

        // Atualiza o contador
        if (CounterManager.Instance != null)
            CounterManager.Instance.Increment();

        Destroy(soul);
    }
}