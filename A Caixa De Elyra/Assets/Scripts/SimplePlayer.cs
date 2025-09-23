using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class SimplePlayer : MonoBehaviour
{
    [Header("Movimento")]
    public float moveSpeed = 5f; // Velocidade do movimento

    private Rigidbody2D rb;

    void Awake()
    {
        // Garante que o Rigidbody2D está presente
        rb = GetComponent<Rigidbody2D>();

        // Configurações do Rigidbody para jogos 2D
        rb.gravityScale = 0;    // Sem gravidade se for top-down
        rb.freezeRotation = true; // Não rotaciona
    }

    void Update()
    {
        // Entrada horizontal (A/D ou setas ← →)
        float move = 0f;
        if (Input.GetKey(KeyCode.LeftArrow) || Input.GetKey(KeyCode.A))
            move = -1f;
        if (Input.GetKey(KeyCode.RightArrow) || Input.GetKey(KeyCode.D))
            move = 1f;

        // Aplica movimento
        rb.linearVelocity = new Vector2(move * moveSpeed, rb.linearVelocity.y);
    }
}

