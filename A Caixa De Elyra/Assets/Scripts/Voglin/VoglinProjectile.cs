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

    // Atualizado: SetDirection garante que o sprite acompanhe corretamente a direção
    public void SetDirection(Vector2 dir)
    {
        direction = dir.normalized;

        // virar sprite conforme direção X
        Vector3 s = transform.localScale;
        s.x = dir.x < 0 ? -Mathf.Abs(s.x) : Mathf.Abs(s.x);
        transform.localScale = s;
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
