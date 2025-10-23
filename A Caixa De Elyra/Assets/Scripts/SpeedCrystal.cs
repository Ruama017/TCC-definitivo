using UnityEngine;

public class SpeedCrystal : MonoBehaviour
{
    public float speedMultiplier = 2f;
    public float duration = 5f;

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            PlayerController player = other.GetComponent<PlayerController>();
            if (player != null)
                player.StartSpeedBoost(speedMultiplier, duration);

            Destroy(gameObject);
        }
    }
}
