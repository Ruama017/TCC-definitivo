using UnityEngine;
using UnityEngine.SceneManagement;

public class ExtraHeartsPickup : MonoBehaviour
{
    public int extraHearts = 5;

    private void OnTriggerEnter2D(Collider2D other)
    {
        PlayerHealth player = other.GetComponent<PlayerHealth>();
        if (player != null && SceneManager.GetActiveScene().name == "Fase3")
        {
            player.AddExtraHearts(extraHearts);
            Destroy(gameObject);
        }
    }
}
