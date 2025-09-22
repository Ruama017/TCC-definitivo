using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [Header("Movimento")]
    public float moveSpeed = 5f;
    public float jumpForce = 7f;

    private Rigidbody2D rb;
    private float moveInput;
    private bool isGrounded;

    [Header("Checagem do chão")]
    public Transform groundCheck;     // Empty no pé do Player
    public float checkRadius = 0.1f;  // tamanho do círculo
    public LayerMask groundLayer;     // Layer do chão/parallax

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    void Update()
    {
        // Movimento horizontal
        moveInput = 0f;
        if (Input.GetKey(KeyCode.Home)) moveInput = -1f;
        if (Input.GetKey(KeyCode.End)) moveInput = 1f;

        // Checa se está no chão
        isGrounded = Physics2D.OverlapCircle(groundCheck.position, checkRadius, groundLayer);

        // Pulo
        if (Input.GetKeyDown(KeyCode.Space) && isGrounded)
        {
            rb.velocity = new Vector2(rb.velocity.x, jumpForce);
        }
    }

    void FixedUpdate()
    {
        // Move horizontal
        rb.velocity = new Vector2(moveInput * moveSpeed, rb.velocity.y);
    }
}


