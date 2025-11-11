using UnityEngine;

public class EnemyHitbox : MonoBehaviour
{
    public int damage = 10;
    public bool applyPoison = false;
    public float poisonDuration = 4f;
    public float poisonTick = 1f;
    public float poisonInterval = 1f;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (!collision.CompareTag("Player")) return;
        PlayerHealth ph = collision.GetComponent<PlayerHealth>();
        if (ph != null)
        {
            ph.TakeDamage(damage);
            if (applyPoison)
                ph.ApplyPoison(poisonDuration, poisonTick, poisonInterval);
        }
    }
}
