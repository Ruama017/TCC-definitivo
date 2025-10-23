using UnityEngine;

public class BoglinController : MonoBehaviour
{
    [Header("Referências")]
    public Transform player;
    public Animator anim;

    [Header("Estados")]
    public BoglinBaseState currentState;
    public BoglinWalkState walkState;
    public BoglinAttackState attackState;

    [Header("Ataque e Detecção")]
    public Transform AttackPoint;
    public Transform SmokeSpawn;
    public Transform DetectionRange;
    public float attackRange = 1f;
    public float moveSpeed = 2f;

    [Header("Saúde")]
    public int maxHealth = 5;
    public int currentHealth;

    [Header("Alma")]
    public GameObject boglinSoulPrefab; 
    public MonsterCounter monsterCounter; 

    void Start()
    {
        currentHealth = maxHealth;
        SwitchState(walkState); // Estado inicial
    }

    void Update()
    {
        if (currentState != null)
            currentState.UpdateState(this);
    }

    public void TakeDamage(int damage)
    {
        currentHealth -= damage;

        if (currentHealth <= 0)
        {
            Die();
        }
    }

    void Die()
    {
        // Instancia a alma e manda para o contador
        if (boglinSoulPrefab != null && monsterCounter != null)
        {
            GameObject soul = Instantiate(boglinSoulPrefab, transform.position, Quaternion.identity);
            Soul soulScript = soul.GetComponent<Soul>();
            if (soulScript != null)
                soulScript.Initialize(monsterCounter.transform.position, monsterCounter);
        }

        Destroy(gameObject);
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
        Vector2 direction = (target - transform.position).normalized;
        transform.position += (Vector3)(direction * moveSpeed * Time.deltaTime);

        if (anim != null)
            anim.SetBool("isWalking", true);

        if (direction.x > 0)
            transform.localScale = new Vector3(1, 1, 1);
        else if (direction.x < 0)
            transform.localScale = new Vector3(-1, 1, 1);
    }

    public void StopMoving()
    {
        if (anim != null)
            anim.SetBool("isWalking", false);
    }
}
