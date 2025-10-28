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

    [Header("Ataque e Detecção")]
    public Transform AttackPoint;
    public float attackRange = 2f;
    public float moveSpeed = 2f;

    [Header("Saúde")]
    public int maxHealth = 5;
    public int currentHealth;

    void Start()
    {
        currentHealth = maxHealth;

        if (rb == null)
            rb = GetComponent<Rigidbody2D>();

        // Criar instâncias dos estados
        walkState = new BoglinWalkState();
        attackState = new BoglinAttackState();

        // Definir estado inicial
        SwitchState(walkState);
    }

    void FixedUpdate()
    {
        if (currentState != null)
            currentState.UpdateState(this);
    }

    public void TakeDamage(int damage)
    {
        currentHealth -= damage;
        if (currentHealth <= 0)
            Die();
    }

    void Die()
    {
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

    // Movimento usando MovePosition
    public void MoveTowards(Vector3 target)
    {
        Vector2 newPos = Vector2.MoveTowards(rb.position, target, moveSpeed * Time.fixedDeltaTime);
        rb.MovePosition(newPos);

        if (anim != null)
            anim.SetBool("IsWalking", true);

        Vector2 direction = (target - transform.position).normalized;

        // Ajuste do sprite para frente
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

    public BoglinWalkState GetWalkState() => walkState;
    public BoglinAttackState GetAttackState() => attackState;

    // Visualizar alcance de ataque no Scene
    void OnDrawGizmos()
    {
        if (AttackPoint != null && attackState != null)
        {
            Gizmos.color = Color.red;
            Gizmos.DrawWireSphere(AttackPoint.position, attackState.attackRadius);
        }
    }
}
