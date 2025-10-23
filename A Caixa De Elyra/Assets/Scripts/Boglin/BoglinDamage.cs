using UnityEngine;

public class BoglinDamage : MonoBehaviour
{
    public int damage = 1;
    private bool hasDealtDamage = false;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        PlayerHealth playerHealth = collision.GetComponent<PlayerHealth>();
        if (playerHealth != null && !hasDealtDamage)
        {
            playerHealth.TakeDamage(damage);
            hasDealtDamage = true;
        }
    }

    public void ResetHit()
    {
        hasDealtDamage = false;
    }
}

