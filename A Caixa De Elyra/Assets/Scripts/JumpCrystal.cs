using UnityEngine;
using System.Collections;

public class JumpCrystal : MonoBehaviour
{
    public float jumpMultiplier = 1.5f; // quanto aumenta o salto
    public float duration = 5f;          // duração em segundos

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            PlayerController player = other.GetComponent<PlayerController>();
            if (player != null)
                player.StartJumpBoost(jumpMultiplier, duration);

            Destroy(gameObject);
        }
    }
}
