using UnityEngine;

public class Soul : MonoBehaviour
{
    public float moveSpeed = 5f;       // Velocidade da alma
    private Vector3 targetPosition;    // Posição do contador
    private MonsterCounter counter;    // Referência para o contador

    // Inicializa a alma
    public void Initialize(Vector3 targetPos, MonsterCounter monsterCounter)
    {
        targetPosition = targetPos;
        counter = monsterCounter;
    }

    void Update()
    {
        if (counter == null) return;

        // Move a alma na direção do contador
        transform.position = Vector3.MoveTowards(transform.position, targetPosition, moveSpeed * Time.deltaTime);

        // Quando chegar no contador
        if (Vector3.Distance(transform.position, targetPosition) < 0.1f)
        {
            counter.AddSoul(); // Atualiza o contador
            Destroy(gameObject); // Destrói a alma
        }
    }
}
