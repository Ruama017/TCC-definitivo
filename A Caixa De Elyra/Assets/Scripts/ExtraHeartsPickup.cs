using UnityEngine;
using UnityEngine.SceneManagement;

public class ExtraHeartsPickup : MonoBehaviour
{
    public int extraHearts = 5;

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player") && SceneManager.GetActiveScene().name == "Cena3")
        {
            PlayerHealth player = other.GetComponent<PlayerHealth>();
            if (player != null)
            {
                player.AddExtraHearts(extraHearts); // adiciona no HUD
                Destroy(gameObject); // destrói coletável
            }
        }
    }
}