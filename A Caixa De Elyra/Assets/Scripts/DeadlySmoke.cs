using UnityEngine;

public class DeadlySmoke : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D collision)
    {
        // Verifica se o player colidiu
        PlayerHealth playerHealth = collision.GetComponent<PlayerHealth>();
        if (playerHealth != null)
        {
            playerHealth.InstantDeath(); // mata o player instantaneamente
        }
    }
}
