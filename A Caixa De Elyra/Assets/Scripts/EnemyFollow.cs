using UnityEngine;

public class EnemyFollow : MonoBehaviour
{
    public Transform player;    // Referência ao player
    public float speed = 2f;    // Velocidade do inimigo
    public float stopDistance = 1.5f; // Distância mínima para parar de chegar muito perto

    void Update()
    {
        if (player == null)
            return;

        // Calcula a distância entre inimigo e player
        float distance = Vector2.Distance(transform.position, player.position);

        // Se estiver longe o suficiente, segue
        if (distance > stopDistance)
        {
            // Move em direção ao player
            transform.position = Vector2.MoveTowards(
                transform.position,
                player.position,
                speed * Time.deltaTime
            );
        }

        // Opcional: virar na direção do player (flip do sprite)
        if (player.position.x > transform.position.x)
            transform.localScale = new Vector3(1, 1, 1);  // olhando pra direita
        else
            transform.localScale = new Vector3(-1, 1, 1); // olhando pra esquerda
    }
}
