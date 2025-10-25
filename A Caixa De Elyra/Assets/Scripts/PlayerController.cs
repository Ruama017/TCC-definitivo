using UnityEngine;
using System.Collections; // necessário para Coroutine

public class PlayerController : MonoBehaviour
{
    [Header("Movimento")]
    public float moveSpeed = 5f;
    public float jumpForce = 12f;

    [Header("Ataque")]
    public Animator animator;
    public GameObject attackHitbox; // Arraste aqui o objeto filho da hitbox da espada
    public float attackDuration = 0.2f; // tempo que a hitbox fica ativa

    private Rigidbody2D rb;
    private bool isGrounded;
    private int extraJumps;
    public int extraJumpValue = 1;

    private bool isAttacking = false; // evita múltiplos ataques ao mesmo tempo
    private bool isDead = false;      // controla se o player morreu

    // ======== SPEED BOOST ========
    [Header("Speed Boost")]
    private float normalSpeed;
    private Coroutine speedBoostCoroutine;

    // ======== JUMP BOOST ========
    [Header("Jump Boost")]
    private float normalJumpForce;
    private Coroutine jumpBoostCoroutine;

    // ======== VIDA ========
    [Header("Player Health")]
    public PlayerHealth playerHealth; // arraste no inspector
    private int currentHealth;

    // expõe isAttacking para outros scripts
    public bool IsAttacking => isAttacking;

    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        extraJumps = extraJumpValue;

        if (attackHitbox != null)
            attackHitbox.SetActive(false); // Começa desativado

        // inicializa variáveis dos boosts
        normalSpeed = moveSpeed;
        normalJumpForce = jumpForce;

        // inicializa vida
        if (playerHealth != null)
            currentHealth = playerHealth.currentHealth;
    }

    private void Update()
    {
        if (isDead) return; // impede qualquer ação após morrer

        Move();
        Jump();
        Attack();
        Flip();

        // Atualiza animação de pulo pelo movimento vertical (mais preciso)
        if (animator != null)
        {
            bool isJumpingAnim = rb.linearVelocity.y != 0 && !isGrounded;
            animator.SetBool("IsJumping", isJumpingAnim);
        }

        // Checa se morreu
        if (playerHealth != null && playerHealth.currentHealth <= 0 && !isDead)
        {
            Die();
        }
    }

    void Move()
    {
        float moveInput = Input.GetAxisRaw("Horizontal");
        rb.linearVelocity = new Vector2(moveInput * moveSpeed, rb.linearVelocity.y);

        if (animator != null)
            animator.SetFloat("Speed", Mathf.Abs(moveInput));
    }

    void Jump()
    {
        if (isGrounded)
            extraJumps = extraJumpValue;

        if (Input.GetKeyDown(KeyCode.Space))
        {
            if (isGrounded || extraJumps > 0)
            {
                rb.linearVelocity = new Vector2(rb.linearVelocity.x, jumpForce);

                if (!isGrounded)
                    extraJumps--;
            }
        }
    }

    void Attack()
    {
        if (Input.GetKeyDown(KeyCode.M) && !isAttacking)
        {
            isAttacking = true;

            if (animator != null)
                animator.SetTrigger("Attack");

            if (attackHitbox != null)
            {
                attackHitbox.SetActive(true);
                Invoke(nameof(DisableHitbox), attackDuration);
            }
        }
    }

    void DisableHitbox()
    {
        if (attackHitbox != null)
            attackHitbox.SetActive(false);

        isAttacking = false; // libera para novo ataque
    }

    void Flip()
    {
        float moveInput = Input.GetAxisRaw("Horizontal");
        if (moveInput != 0)
            transform.localScale = new Vector3(Mathf.Sign(moveInput) * Mathf.Abs(transform.localScale.x), transform.localScale.y, 1);
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Ground"))
            isGrounded = true;
    }

    private void OnCollisionExit2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Ground"))
            isGrounded = false;
    }

    // ======== MÉTODOS DE SPEED BOOST ========
    public void StartSpeedBoost(float multiplier, float duration)
    {
        if (speedBoostCoroutine != null)
            StopCoroutine(speedBoostCoroutine);

        speedBoostCoroutine = StartCoroutine(SpeedBoost(multiplier, duration));
    }

    private IEnumerator SpeedBoost(float multiplier, float duration)
    {
        moveSpeed = normalSpeed * multiplier;

        SpriteRenderer sr = GetComponent<SpriteRenderer>();
        if (sr != null)
            sr.color = Color.cyan;

        yield return new WaitForSeconds(duration);

        moveSpeed = normalSpeed;

        if (sr != null)
            sr.color = Color.white;

        speedBoostCoroutine = null;
    }

    // ======== MÉTODOS DE JUMP BOOST ========
    public void StartJumpBoost(float multiplier, float duration)
    {
        if (jumpBoostCoroutine != null)
            StopCoroutine(jumpBoostCoroutine);

        jumpBoostCoroutine = StartCoroutine(JumpBoost(multiplier, duration));
    }

    private IEnumerator JumpBoost(float multiplier, float duration)
    {
        jumpForce = normalJumpForce * multiplier;

        SpriteRenderer sr = GetComponent<SpriteRenderer>();
        if (sr != null)
            sr.color = Color.yellow;

        yield return new WaitForSeconds(duration);

        jumpForce = normalJumpForce;

        if (sr != null)
            sr.color = Color.white;

        jumpBoostCoroutine = null;
    }

    // ======== MÉTODO DE DANO ========
    public void TakeDamage(int damage)
    {
        if (playerHealth != null)
        {
            playerHealth.TakeDamage(damage);
            currentHealth = playerHealth.currentHealth;
        }
    }

    // ======== MÉTODO DE MORTE ========
    private void Die()
    {
        isDead = true;
        rb.linearVelocity = Vector2.zero; // para o movimento

        if (animator != null)
            animator.SetTrigger("Death"); // toca animação de morte

        // Desativa o hitbox de ataque
        if (attackHitbox != null)
            attackHitbox.SetActive(false);
    }
}
