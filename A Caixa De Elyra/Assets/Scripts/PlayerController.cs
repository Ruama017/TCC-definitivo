using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
[RequireComponent(typeof(Animator))]
public class PlayerController : MonoBehaviour
{
    [Header("Configurações de Movimento")]
    public float moveSpeed = 5f;

    private Rigidbody2D rb;
    private Animator animator;
    private float moveInput;

    void Awake()
    {
        // Pega os componentes obrigatórios
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
    }

    void Update()
    {
        // Captura entrada horizontal do teclado
        moveInput = Input.GetAxisRaw("Horizontal"); // -1, 0 ou 1

        // Atualiza parâmetro "speed" do Animator
        animator.SetFloat("speed", Mathf.Abs(moveInput));
    }

    void FixedUpdate()
    {
        // Move o player horizontalmente mantendo a velocidade vertical
        rb.velocity = new Vector2(moveInput * moveSpeed, rb.velocity.y);
    }
}


