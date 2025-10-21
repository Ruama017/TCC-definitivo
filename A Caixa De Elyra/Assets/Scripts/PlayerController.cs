using UnityEngine;

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

    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        extraJumps = extraJumpValue;

        if (attackHitbox != null)
            attackHitbox.SetActive(false); // Começa desativado
    }

    private void Update()
    {
        Move();
        Jump();
        Attack();
        Flip();

        // Atualiza animação de pulo pelo movimento vertical (mais preciso)
        if (animator != null)
        {
            bool isJumpingAnim = rb.velocity.y != 0 && !isGrounded;
            animator.SetBool("IsJumping", isJumpingAnim);
        }
    }

    void Move()
    {
        float moveInput = Input.GetAxisRaw("Horizontal");
        rb.velocity = new Vector2(moveInput * moveSpeed, rb.velocity.y);

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
                rb.velocity = new Vector2(rb.velocity.x, jumpForce);

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
        {
            isGrounded = true;
        }
    }

    private void OnCollisionExit2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Ground"))
            isGrounded = false;
    }
}