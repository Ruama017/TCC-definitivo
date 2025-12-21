using UnityEngine;

public class BoglinController : MonoBehaviour
{
    [Header("Referências")]
    public Transform player;
    public Animator anim;
    public Rigidbody2D rb;

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
    }

    void Update()
    {
        
    }

    
    // MOVIMENTO (usado pelos States)
   
    public void MoveTowards(Vector3 target)
    {
        Vector2 targetPos = new Vector2(target.x, rb.position.y);
        Vector2 newPos = Vector2.MoveTowards(
            rb.position,
            targetPos,
            moveSpeed * Time.deltaTime
        );

        rb.MovePosition(newPos);

        // Flip
        if (target.x > transform.position.x)
            transform.localScale = new Vector3(-1, 1, 1);
        else
            transform.localScale = new Vector3(1, 1, 1);
    }

    public void StopMoving()
    {
        rb.velocity = Vector2.zero;
    }

    
    
    // ATAQUE
    
    
    public void PlayAttackSound()
    {
        if (attackSound != null)
            attackSound.Play();
    }

   
   
    // VIDA
    
    
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
}
