using UnityEngine;
using System.Collections;

public class PlayerController : MonoBehaviour
{
    [Header("Movimento")]
    public float moveSpeed = 5f;
    public float jumpForce = 12f;

    [Header("Ataque")]
    public Animator animator;
    public GameObject attackHitbox; 
    public float attackDuration = 0.2f;

    [Header("SFX de Pulo")]
    public AudioSource jumpSound;

    // --- ADIÇÃO: sons extras ---
    [Header("SFX Extras")]
    public AudioSource attackSound;
    public AudioSource hurtSound;
    public AudioSource deathSound;
    public AudioSource superSpeedSound;
    // ----------------------------

    private Rigidbody2D rb;
    private bool isGrounded;
    private int extraJumps;
    public int extraJumpValue = 1;

    private bool isAttacking = false;
    private bool isDead = false;

    [Header("Speed Boost")]
    private float normalSpeed;
    private Coroutine speedBoostCoroutine;

    [Header("Jump Boost")]
    private float normalJumpForce;
    private Coroutine jumpBoostCoroutine;

    [Header("Player Health")]
    public PlayerHealth playerHealth; 
    private int currentHealth;

    [Header("Super - Bota")]
    public bool hasSuper = false;        
    public GameObject superEffect;       
    public PlayerSuper playerSuper;

    [Header("Super - Ataque")]
    public bool hasAttackSuper = false;  
    public GameObject attackSuperEffect; 

    public bool IsAttacking => isAttacking;

    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        extraJumps = extraJumpValue;

        if (attackHitbox != null)
            attackHitbox.SetActive(false);

        normalSpeed = moveSpeed;
        normalJumpForce = jumpForce;

        if (playerHealth != null)
            currentHealth = playerHealth.currentHealth;

        if (playerSuper == null)
            playerSuper = GetComponentInChildren<PlayerSuper>();
    }

    private void Update()
    {
        if (isDead) return;

        if (playerSuper != null)
            hasSuper = playerSuper.isSuper;

        Move();
        Jump();
        Attack();
        Flip();

        if (animator != null)
        {
            animator.SetFloat("Speed", Mathf.Abs(rb.velocity.x));
            animator.SetBool("IsGrounded", isGrounded);
            animator.SetFloat("yVelocity", rb.velocity.y);

            bool isJumping = !isGrounded && rb.velocity.y > 0.1f;
            bool isFalling = !isGrounded && rb.velocity.y < -0.1f;

            animator.SetBool("IsJumping", isJumping);
            animator.SetBool("IsFalling", isFalling);
        }

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

    void Attack()
    {
        if (Input.GetKeyDown(KeyCode.M) && !isAttacking)
        {
            isAttacking = true;

            // --- SFX: ataque ---
            if (attackSound != null)
                attackSound.Play();
            // -------------------

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

                ThorneBossController thorne = hit.GetComponent<ThorneBossController>();
                NitroMortis nitro = hit.GetComponent<NitroMortis>();

                if (thorne != null)
                    thorne.TakeDamage(1, hasSuper);

                if (nitro != null)
                {
                    // ✅ Alteração: agora passa apenas bool
                    nitro.TakeDamage(hasSuper);
                }
            }

            attackHitbox.SetActive(false);
        }

        isAttacking = false;
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

    public void StartSpeedBoost(float multiplier, float duration)
    {
        if (speedBoostCoroutine != null)
            StopCoroutine(speedBoostCoroutine);

        // --- SFX: super speed ---
        if (superSpeedSound != null)
            superSpeedSound.Play();
        // ------------------------

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

    public void TakeDamage(int damage)
    {
        if (hurtSound != null)
            hurtSound.Play();

        if (playerHealth != null)
        {
            playerHealth.TakeDamage(damage);
            currentHealth = playerHealth.currentHealth;
        }
    }

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

    public void ActivateAttackSuper(float duration)
    {
        hasAttackSuper = true;

        if (attackSuperEffect != null)
            attackSuperEffect.SetActive(true);

        StartCoroutine(DeactivateAttackSuperAfterTime(duration));
    }

    private IEnumerator DeactivateAttackSuperAfterTime(float time)
    {
        yield return new WaitForSeconds(time);
        hasAttackSuper = false;

        if (attackSuperEffect != null)
            attackSuperEffect.SetActive(false);
    }

    private void Die()
    {
        isDead = true;
        rb.velocity = Vector2.zero;

        if (deathSound != null)
            deathSound.Play();

        if (animator != null)
            animator.SetTrigger("Death");

        if (attackHitbox != null)
            attackHitbox.SetActive(false);
    }
}
