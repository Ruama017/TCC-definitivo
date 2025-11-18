using UnityEngine;

public class EnemyProjectile : MonoBehaviour
{
    public int damage = 2;        // dano que o projetil causa
    public float lifetime = 3f;   // destrói sozinho após X segundos

    void Start()
    {
        Destroy(gameObject, lifetime); // destrói automaticamente
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            // Assumindo que o Player tem um script PlayerHealth com TakeDamage()
            PlayerHealth playerHealth = other.GetComponent<PlayerHealth>();
            if (playerHealth != null)
                playerHealth.TakeDamage(damage);

            Destroy(gameObject); // destrói projetil ao atingir o player
        }
    }
}
