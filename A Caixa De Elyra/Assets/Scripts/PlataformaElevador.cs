using UnityEngine;

public class PlataformaElevador : MonoBehaviour
{
    public Transform pontoSuperior;
    public Transform pontoInferior;
    public float velocidade = 2f;

    private bool playerEmCima = false;
    private Transform playerTransform;
    private Vector3 posAnterior;

    void Start()
    {
        posAnterior = transform.position;
    }

    void Update()
    {
        Vector3 destino = playerEmCima ? pontoSuperior.position : pontoInferior.position;
        transform.position = Vector3.MoveTowards(transform.position, destino, velocidade * Time.deltaTime);

        // Move o player junto com a plataforma
        if (playerEmCima && playerTransform != null)
        {
            Vector3 delta = transform.position - posAnterior;
            playerTransform.position += delta;
        }

        posAnterior = transform.position;
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.collider.CompareTag("Player"))
        {
            playerEmCima = true;
            playerTransform = collision.collider.transform;
        }
    }

    private void OnCollisionExit2D(Collision2D collision)
    {
        if (collision.collider.CompareTag("Player"))
        {
            playerEmCima = false;
            playerTransform = null;
        }
    }
}
