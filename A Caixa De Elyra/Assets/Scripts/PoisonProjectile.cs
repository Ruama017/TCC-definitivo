using UnityEngine;

public class PoisonProjectile : MonoBehaviour
{
    public float speed = 6f;
    public float lifeTime = 5f;

    public int damage = 1;
    public float poisonDuration = 4f;
    public float poisonTick = 1f;
    public float poisonInterval = 1f;

    private Vector2 direction;

    public void SetDirection(Vector2 dir)
    {
        direction = dir.normalized;
    }

    void Start()
    {
        Destroy(gameObject, lifeTime);
    }

    void Update()
    {
        transform.Translate(direction * speed * Time.deltaTime);
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            PlayerHealth ph = other.GetComponent<PlayerHealth>();
            if (ph != null)
            {
                ph.TakeDamage(damage);
                ph.ApplyPoison(poisonDuration, poisonTick, poisonInterval);
            }
        }

        Destroy(gameObject);
    }
}
