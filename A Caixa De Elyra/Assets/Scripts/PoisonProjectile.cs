using UnityEngine;

public class PoisonProjectile : MonoBehaviour
{
    public float speed = 8f;
    public float lifeTime = 5f;
    public int damage = 3;
    public float poisonDuration = 4f;
    public float poisonTick = 1f;
    public float poisonInterval = 1f;
    private Vector2 dir;

    public void SetDirection(Vector2 d) => dir = d.normalized;

    void Start() => Destroy(gameObject, lifeTime);

    void Update() => transform.Translate(dir * speed * Time.deltaTime, Space.World);

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (!other.CompareTag("Player")) { Destroy(gameObject); return; }

        PlayerHealth ph = other.GetComponent<PlayerHealth>();
        if (ph != null)
        {
            ph.TakeDamage(damage);
            ph.ApplyPoison(poisonDuration, poisonTick, poisonInterval);
        }
        Destroy(gameObject);
    }
}
