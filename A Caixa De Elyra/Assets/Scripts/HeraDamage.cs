using UnityEngine;

public class HeraDamage : MonoBehaviour
{
    public int damage = 1;
    private bool isDestroyed = false; // evita múltiplos golpes
    private Animator anim;

    private void Awake()
    {
        anim = GetComponent<Animator>();
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        PlayerHealth playerHealth = collision.gameObject.GetComponent<PlayerHealth>();
        if (playerHealth != null)
        {
            playerHealth.TakeDamage(damage);
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        PlayerHealth playerHealth = collision.GetComponent<PlayerHealth>();
        if (playerHealth != null)
        {
            playerHealth.TakeDamage(damage);
        }
    }

    public void TakeDamage(int damage)
    {
        if (isDestroyed) return; // já destruída, não repete
        isDestroyed = true;

        if (anim != null)
        {
            anim.SetTrigger("Destroy"); // usa o mesmo nome do Animator
        }

        // desativa o collider pra não ser atacada de novo
        Collider2D col = GetComponent<Collider2D>();
        if (col != null)
            col.enabled = false;

        // destrói após a animação (ajuste conforme a duração da animação)
        Destroy(gameObject, 0.6f);
    }
}
