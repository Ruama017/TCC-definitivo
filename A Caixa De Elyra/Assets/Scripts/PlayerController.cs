using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class PlayerController : MonoBehaviour
{
    [Header("Movimento")]
    public float moveSpeed = 6f;

    [Header("Pulo")]
    public float jumpForce = 12f;
    public float coyoteTime = 0.12f;        // tempo que ainda pode pular após sair do chão
    public float jumpBufferTime = 0.12f;    // tempo que o input de pulo é lembrado

    [Header("Checagem do chão")]
    public Transform groundCheck;
    public float groundCheckRadius = 0.12f;
    public LayerMask groundLayer;

    private Rigidbody2D rb;
    private float horizInput;
    private float lastGroundedTime = -1f;
    private float lastJumpPressedTime = -1f;
    private bool facingRight = true;

    void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    void Update()
    {
        // Entrada horizontal (setas/A D) + suporte extra para Home/End (se preferir)
        horizInput = Input.GetAxisRaw("Horizontal"); // -1,0,1 via Input Manager
        if (Input.GetKey(KeyCode.Home)) horizInput = -1f;
        if (Input.GetKey(KeyCode.End))  horizInput = 1f;

        // Captura pulo (buffer)
        if (Input.GetKeyDown(KeyCode.Space))
            lastJumpPressedTime = Time.time;

        // Atualiza grounded (via overlap)
        bool groundedNow = Physics2D.OverlapCircle(groundCheck.position, groundCheckRadius, groundLayer);
        if (groundedNow) lastGroundedTime = Time.time;

        // Tenta executar pulo usando jump buffer + coyote time
        if (Time.time - lastJumpPressedTime <= jumpBufferTime &&
            Time.time - lastGroundedTime <= coyoteTime)
        {
            // executa pulo
            rb.linearVelocity = new Vector2(rb.linearVelocity.x, jumpForce);
            lastJumpPressedTime = -999f;
            lastGroundedTime = -999f;
        }

        // vira sprite conforme direção
        if (horizInput > 0 && !facingRight) Flip();
        else if (horizInput < 0 && facingRight) Flip();
    }

    void FixedUpdate()
    {
        // Aplica movimento horizontal (via velocity)
        rb.linearVelocity = new Vector2(horizInput * moveSpeed, rb.linearVelocity.y);
    }

    void Flip()
    {
        facingRight = !facingRight;
        Vector3 s = transform.localScale;
        s.x *= -1f;
        transform.localScale = s;
    }

    void OnDrawGizmosSelected()
    {
        if (groundCheck != null)
        {
            Gizmos.color = Color.yellow;
            Gizmos.DrawWireSphere(groundCheck.position, groundCheckRadius);
        }
    }
}

