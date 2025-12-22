using UnityEngine;

public class BoglinController : MonoBehaviour
{
    [Header("Referências")]
    public Transform player;
    public Animator anim;
    public Rigidbody2D rb;

    [Header("Movimento")]
    public float moveSpeed = 2f;

    [Header("Combate")]
    public float detectionRange = 5f;
    public float attackRange = 1.5f;

    [Header("Patrulha")]
    public Transform leftPatrolPoint;
    public Transform rightPatrolPoint;

    [Header("Vida")]
    public int maxHealth = 5;
    public int currentHealth;

    [Header("Sons do Boglin")]
    public AudioSource attackSound;
    public AudioSource deathSound;

    
    private int lastDirection = -1;

    void Start()
    {
        currentHealth = maxHealth;

        if (anim != null)
        {
            anim.enabled = true;
            anim.speed = 1f;
        }
    }

    public void MoveTowards(Vector3 target)
    {
        float deltaX = target.x - transform.position.x;

        Debug.Log(deltaX);

        //if (Mathf.Abs(deltaX) < 0.1f)
        //{
        //    rb.velocity = new Vector2(0, rb.velocity.y);
        //    return;
        //}

        int direction = deltaX > 0 ? 1 : -1;

        rb.velocity = new Vector2(direction * moveSpeed, rb.velocity.y);

        
        if (direction != lastDirection)
        {
            Vector3 scale = transform.localScale;

            if (direction == -1)
                scale.x = Mathf.Abs(scale.x);   // esquerda
            else
                scale.x = -Mathf.Abs(scale.x);  // direita

            transform.localScale = scale;
            lastDirection = direction;
        }

        anim.SetBool("isWalkin", true);
    }

    public void StopMoving()
    {
        rb.velocity = new Vector2(0, rb.velocity.y);
        anim.SetBool("isWalkin", false);
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
    {
        Die();
    }
}


    public void Die()
    {
        if (deathSound != null)
            deathSound.Play();

        Destroy(gameObject);
    }
}
