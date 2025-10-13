using UnityEngine;

public class HeraEspinhos : MonoBehaviour
{
    public int damage = 1; // Quantos corações o player perde ao tocar

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            PlayerHealth ph = collision.GetComponent<PlayerHealth>();
            if (ph != null)
            {
                ph.TakeDamage(damage);
            }
        }
    }
}