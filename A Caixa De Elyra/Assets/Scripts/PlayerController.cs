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

    [Header("SFX de Pulo")]
    public AudioSource jumpSound; // Som do pulo

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

    // ======== SUPER ========
    [Header("Super")]
    public bool hasSuper = false;        // true se o player coletou o Super
    public GameObject superEffect;       // efeitos de brilhos

    // expõe isAttacking para outros scripts
    public bool IsAttacking => isAttacking;

    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        extraJumps = extraJumpValue;

        if (attackHitbox != null)
            attackHitbox.SetActive(false); // Começa desativado

        normalSpeed = moveSpeed;
        normalJumpForce = jumpForce;

        if (playerHealth != null)
            currentHealth = playerHealth.currentHealth;
    }

    private void Update()
    {
        if (isDead) return;

        Move();
        Jump();
        Attack();
        Flip();

        // Atualiza animações
        if (animator != null)
        {
            animator.SetFloat("Speed", Mathf.Abs(rb.velocity.x)); // andando
            animator.SetBool("IsGrounded", isGrounded);
            animator.SetFloat("yVelocity", rb.velocity.y);

            bool isJumping = !isGrounded && rb.velocity.y > 0.1f;
            bool isFalling = !isGrounded && rb.velocity.y < -0.1f;

            animator.SetBool("IsJumping", isJumping);
            animator.SetBool("IsFalling", isFalling);
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
        rb.velocity = new Vector2(moveInput * moveSpeed, rb.velocity.y);
    }

    void Jump()
    {
        if (isGrounded)
            extraJumps = extraJumpValue;

        if (Input.GetKeyDown(KeyCode.Space))
        {
            if (isGrounded || extraJumps > 0)
            {
                rb.velocity = new Vector2(rb.velocity.x, jumpForce);

                if (!isGrounded)
                    extraJumps--;

                if (animator != null)
                    animator.SetTrigger("Jump");

                if (jumpSound != null)
                    jumpSound.Play();
            }
        }
    }

    // ======================== ATAQUE ========================
    void Attack()
    {
        if (Input.GetKeyDown(KeyCode.M) && !isAttacking)
        {
            isAttacking = true;

            if (animator != null)
            {
                animator.ResetTrigger("Attack");
                animator.SetTrigger("Attack");
                animator.Update(0f);
            }

            if (attackHitbox != null)
            {
                attackHitbox.SetActive(true);
                StartCoroutine(DisableHitboxAfterDelay(attackDuration));
            }
        }
    }

    private IEnumerator DisableHitboxAfterDelay(float delay)
    {
        yield return new WaitForSeconds(delay);

        if (attackHitbox != null)
        {
            Collider2D[] hits = Physics2D.OverlapBoxAll(attackHitbox.transform.position, attackHitbox.transform.localScale, 0f);
            foreach (Collider2D hit in hits)
            {
                if (hit.CompareTag("Player")) continue;

                // Dano nos bosses
                ThorneBossController thorne = hit.GetComponent<ThorneBossController>();
                NitroMortis nitro = hit.GetComponent<NitroMortis>();

                if (thorne != null)
                {
                    thorne.TakeDamage(1, hasSuper);
                    Debug.Log("[DEBUG] Ataque do player atingiu Thorne! Super: " + hasSuper);
                }

                if (nitro != null)
                {
                    int dano = 1; // dano base
                    if (hasSuper) dano = 2; // se tiver Super, aumenta dano
                    nitro.TakeDamage(dano);
                    Debug.Log("[DEBUG] Ataque do player atingiu NitroMortis! Dano: " + dano);
                }
            }

            attackHitbox.SetActive(false);
        }

        isAttacking = false;
    }
    // ========================================================

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

    // ======== SPEED BOOST ========
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

    // ======== JUMP BOOST ========
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

    // ======== SUPER ========
    public void ActivateSuper(float duration)
    {
        hasSuper = true;

        if (superEffect != null)
            superEffect.SetActive(true);

        StartCoroutine(DeactivateSuperAfterTime(duration));
    }

    private IEnumerator DeactivateSuperAfterTime(float time)
    {
        yield return new WaitForSeconds(time);
        hasSuper = false;

        if (superEffect != null)
            superEffect.SetActive(false);
    }

    // ======== MORTE ========
    private void Die()
    {
        isDead = true;
        rb.velocity = Vector2.zero;

        if (animator != null)
            animator.SetTrigger("Death");

        if (attackHitbox != null)
            attackHitbox.SetActive(false);
    }
}
