using UnityEngine;

public class NitroClawDamage : MonoBehaviour
{
    public int damage = 3; // dano da garra

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            PlayerHealth playerHealth = other.GetComponent<PlayerHealth>();
            if (playerHealth != null)
                playerHealth.TakeDamage(damage);
        }
    }
}
