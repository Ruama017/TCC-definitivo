using UnityEngine;
using System.Collections;

public class BoglinSoulController : MonoBehaviour
{
    public GameObject soulPrefab;      // prefab da alma
    public float speed = 5f;           // velocidade da alma

    private Vector3 targetPosition;    // posição do contador

    void Start()
    {
        // Define a posição fixa do contador
        targetPosition = new Vector3(562f, 499f, 0f); // X=562, Y=499
    }

    public void FlyToCounter(Vector3 startPosition)
    {
        StartCoroutine(MoveSoul(startPosition));
    }

    private IEnumerator MoveSoul(Vector3 startPos)
    {
        // Instancia a alma na posição do Boglin
        GameObject soul = Instantiate(soulPrefab, startPos, Quaternion.identity);
        soul.SetActive(true);

        // Move até a posição do contador
        while (Vector3.Distance(soul.transform.position, targetPosition) > 0.05f)
        {
            soul.transform.position = Vector3.MoveTowards(soul.transform.position, targetPosition, speed * Time.deltaTime);
            yield return null;
        }

        // Chegou no contador
        Destroy(soul);

        // Atualizar contador (aqui você chama seu script que atualiza a UI)
        CounterManager.Instance.Increment();
    }
}
