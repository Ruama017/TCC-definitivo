using UnityEngine;

public class EnemyFollow : MonoBehaviour
{
    public float speed = 2f;            // velocidade de movimento
    public float detectionRange = 5f;   // distância máxima pra começar a seguir
    public float stopDistance = 1f;     // distância mínima pra parar de se aproximar
    private Transform player;           // referência ao jogador
    private Rigidbody2D rb;             // corpo do inimigo

    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player").transform;
        rb = GetComponent<Rigidbody2D>();
    }

    void Update()
    {
        if (player == null) return;

        float distance = Vector2.Distance(transform.position, player.position);

        if (distance < detectionRange && distance > stopDistance)
        {
            // direção normalizada em direção ao player
            Vector2 direction = (player.position - transform.position).normalized;
            
            // move o inimigo na direção do jogador
            rb.MovePosition(rb.position + direction * speed * Time.deltaTime);

            // opcional: vira o inimigo de acordo com o lado do player
            if (direction.x > 0)
                transform.localScale = new Vector3(1, 1, 1);
            else
                transform.localScale = new Vector3(-1, 1, 1);
        }
    }

    void OnDrawGizmosSelected()
    {
        // Mostra o raio de detecção no editor
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, detectionRange);
    }
}
