using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [Header("Movimento")]
    public float moveSpeed = 5f;
    public float jumpForce = 7f;

    private Rigidbody2D rb;
    private Animator animator;
    private bool isGrounded;
    private bool isAttacking;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
    }

    void Update()
    {
        if (isAttacking) return; // enquanto ataca, não move

        // Movimento horizontal
        float moveInput = Input.GetAxisRaw("Horizontal"); // A/D ou setas
        rb.linearVelocity = new Vector2(moveInput * moveSpeed, rb.linearVelocity.y);

        // Flip no sprite para olhar na direção certa
        if (moveInput != 0)
        {
            transform.localScale = new Vector3(Mathf.Sign(moveInput), 1f, 1f);
        }

        // Pular
        if (Input.GetButtonDown("Jump") && isGrounded)
        {
            rb.linearVelocity = new Vector2(rb.linearVelocity.x, jumpForce);
        }

        // Atacar
        if (Input.GetKeyDown(KeyCode.F)) // usa a tecla F para atacar
        {
            StartAttack();
        }

        // Atualizar parâmetros do Animator
        animator.SetBool("isWalking", moveInput != 0);
        animator.SetBool("isJumping", !isGrounded);
    }

    // Função de ataque
    private void StartAttack()
    {
        isAttacking = true;
        animator.SetTrigger("Attack");
    }

    // Chamado pelo final da animação de ataque (Animation Event)
    public void EndAttack()
    {
        isAttacking = false;
    }

    // Verifica se está no chão
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
}
