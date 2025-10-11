using UnityEngine;

public class MovingPlatform : MonoBehaviour
{
    [Header("Configuração de movimento")]
    public Vector3 moveDirection = Vector3.right; // direção inicial (X ou Y)
    public float moveDistance = 3f;               // distância total do vai-e-vem
    public float moveSpeed = 2f;                  // velocidade de movimento
    public float oscillationAmplitude = 0.2f;     // quanto ela balança antes de mudar
    public float oscillationSpeed = 5f;           // velocidade da oscilação

    private Vector3 startPos;
    private Vector3 targetPos;
    private bool movingToTarget = true;

    void Start()
    {
        startPos = transform.position;
        targetPos = startPos + moveDirection.normalized * moveDistance;
    }

    void Update()
    {
        MovePlatform();
        ApplyOscillation();
    }

    void MovePlatform()
    {
        if (movingToTarget)
        {
            transform.position = Vector3.MoveTowards(transform.position, targetPos, moveSpeed * Time.deltaTime);
            if (Vector3.Distance(transform.position, targetPos) < 0.01f)
                movingToTarget = false;
        }
        else
        {
            transform.position = Vector3.MoveTowards(transform.position, startPos, moveSpeed * Time.deltaTime);
            if (Vector3.Distance(transform.position, startPos) < 0.01f)
                movingToTarget = true;
        }
    }

    void ApplyOscillation()
    {
        // Oscilação perpendicular à direção principal do movimento
        Vector3 perpendicular = new Vector3(-moveDirection.y, moveDirection.x, moveDirection.z).normalized;
        Vector3 oscillationOffset = perpendicular * Mathf.Sin(Time.time * oscillationSpeed) * oscillationAmplitude;

        transform.position += oscillationOffset;
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
            collision.transform.SetParent(transform); // jogador “gruda” na plataforma
    }

    private void OnCollisionExit2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
            collision.transform.SetParent(null); // jogador solta a plataforma
    }
}
