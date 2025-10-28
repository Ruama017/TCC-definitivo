using UnityEngine;

public class VoglinProjectile : MonoBehaviour
{
    public float speed = 6f;
    public int damage = 1;
    public float lifeTime = 4f; // tempo até se destruir sozinho
    private Vector2 direction;

    void Start()
    {
        Destroy(gameObject, lifeTime);
    }

    void Update()
    {
        transform.Translate(direction * speed * Time.deltaTime);
    }

    public void SetDirection(Vector2 dir)
    {
        direction = dir.normalized;
        // virar sprite conforme direção X
        if (dir.x < 0)
        {
            Vector3 s = transform.localScale;
            s.x *= -1;
            transform.localScale = s;
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            PlayerHealth player = collision.GetComponent<PlayerHealth>();
            if (player != null)
            {
                player.TakeDamage(damage);
            }
            Destroy(gameObject);
        }

        // se bater em parede, etc
        if (collision.CompareTag("Ground") || collision.gameObject.layer == LayerMask.NameToLayer("Ground"))
        {
            Destroy(gameObject);
        }
    }
}
