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
    public LayerMask groundLayer;     // Layer do chão

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        if(rb == null) rb = gameObject.AddComponent<Rigidbody2D>();
        
        rb.gravityScale = 3; // para poder cair
        rb.freezeRotation = true;

        if(GetComponent<BoxCollider2D>() == null)
            gameObject.AddComponent<BoxCollider2D>();
    }

    void Update()
    {
        // Movimento horizontal (usa Input Manager: A/D ou setas)
        moveInput = Input.GetAxisRaw("Horizontal");

        // Checa se está no chão
        isGrounded = Physics2D.OverlapCircle(groundCheck.position, checkRadius, groundLayer);

        // Pulo
        if(Input.GetKeyDown(KeyCode.Space) && isGrounded)
        {
            rb.AddForce(Vector2.up * jumpForce, ForceMode2D.Impulse);
        }

        // Pulo responsivo: corta altura se soltar espaço
        if (rb.linearVelocity.y > 0 && !Input.GetKey(KeyCode.Space))
        {
            rb.linearVelocity += Vector2.up * Physics2D.gravity.y * (2f * Time.deltaTime);
        }
    }

    void FixedUpdate()
    {
        rb.linearVelocity = new Vector2(moveInput * moveSpeed, rb.linearVelocity.y);
    }
}
