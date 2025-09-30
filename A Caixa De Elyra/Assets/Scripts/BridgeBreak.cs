using UnityEngine;

public class BridgeBreak : MonoBehaviour
{
    [Header("Pedaços da Ponte")]
    public Rigidbody2D[] pecasDaPonte; // Arraste os pedaços aqui no Inspector
    public float forcaExplosao = 2f;   // Força da quebra

    private bool jaQuebrou = false;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (!jaQuebrou && collision.CompareTag("Player"))
        {
            jaQuebrou = true;

            foreach (Rigidbody2D peca in pecasDaPonte)
            {
                peca.bodyType = RigidbodyType2D.Dynamic; // Faz a peça cair
                peca.AddForce(Random.insideUnitCircle * forcaExplosao, ForceMode2D.Impulse);
                peca.AddTorque(Random.Range(-5f, 5f), ForceMode2D.Impulse); // Faz girar
            }
        }
    }
}
