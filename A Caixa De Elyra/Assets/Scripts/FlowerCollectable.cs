using UnityEngine;

public class FlowerCollectable : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D other)
    {
        if(other.CompareTag("Player"))
        {
            // Ativa escudo no PlayerHealth
            PlayerHealth player = other.GetComponent<PlayerHealth>();
            if(player != null)
            {
                player.ActivateShield();
                Destroy(gameObject); // Remove a flor
            }
        }
    }
}
